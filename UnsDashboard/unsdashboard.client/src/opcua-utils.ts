/* eslint-disable @typescript-eslint/no-explicit-any */
import { Account } from './api';
import * as OpcUa from './opcua';

export class HandleFactory {
   private static counter: number = 0;
   public static increment(): number {
      return ++this.counter;
   }
}

export interface IServiceFault {
   code?: number;
   message?: string;
}

interface ServiceFault {
   ResponseHeader?: OpcUa.ResponseHeader;
}

interface IServiceFaultMessage {
   NamespaceUris?: Array<string>;
   ServerUris?: Array<string>;
   ServiceId?: number;
   Body: ServiceFault;
}

interface IServiceRequestMessage {
   NamespaceUris?: Array<string>;
   ServerUris?: Array<string>;
   ServiceId?: number;
   Body?: any;
}

async function readResponseBody(url: string, response: any) {
   const content = response.headers.get("Content-Type");

   if (content && content.indexOf("json") < 0) {
      console.error("URL: " + url);
      console.error("UnexpectedResponse: " + await response.text());
      return null;
   }

   return await response.json();
}

function toFault(response?: IServiceFaultMessage): IServiceFault | null {
   if (response?.Body?.ResponseHeader) {
      const responseHeader = response.Body.ResponseHeader;
      if (!responseHeader.ServiceResult || responseHeader.ServiceResult === 0) {
         return null;
      }
      const stringTable = response.Body?.ResponseHeader?.StringTable;
      const serviceDiagnostics = response.Body?.ResponseHeader?.ServiceDiagnostics;
      if (!stringTable?.length || !serviceDiagnostics) {
         return { code: responseHeader.ServiceResult };
      }
      let message = '';
      if ((serviceDiagnostics?.SymbolicId && serviceDiagnostics?.SymbolicId > 0) || serviceDiagnostics?.SymbolicId === 0) {
         message += `[${stringTable?.[serviceDiagnostics?.SymbolicId]}] `;
      }
      if ((serviceDiagnostics?.LocalizedText && serviceDiagnostics?.LocalizedText > 0) || serviceDiagnostics?.LocalizedText === 0) {
         message += `'${stringTable?.[serviceDiagnostics?.LocalizedText]}'`;
      }
      return {
         code: responseHeader.ServiceResult,
         message: message
      }
   }
   return {
      code: OpcUa.StatusCodes.BadUnknownResponse
   }
}
export async function call(
   url: string,
   request: IServiceRequestMessage,
   controller?: AbortController,
   user?: Account) {

   const timeoutId = (request?.Body?.RequestHeader?.TimeoutHint && controller)
      ? setTimeout(() => controller.abort(), request?.Body?.RequestHeader?.TimeoutHint)
      : null;
   try {
      const requestOptions = {
         method: 'POST',
         headers: {
            'Content-Type': 'application/json',
            'Authorization': (user?.accessToken)?`Bearer ${user?.accessToken}`:''
         },
         body: JSON.stringify(request),
         signal: controller?.signal
      };
      console.info(`call: ${url}`);
      const response = await fetch(url, requestOptions);
      if (timeoutId) clearTimeout(timeoutId);
      if (response.ok) {
         const body = await readResponseBody(url, response);
         const fault = toFault(body);
         if (fault) {
            return fault;
         }
         return body;
      }
      else {
         console.info(`call: ${response.status} ${response.statusText}`);
         return {
            code: OpcUa.StatusCodes.BadUnexpectedError,
            message: `HTTP ${response.status} ${response.statusText}`
         };
      }
   }
   catch (exception: any) {
      if (timeoutId) clearTimeout(timeoutId);
      if (exception.code) {
         console.info(`call: ${exception.code} ${exception.message}`);
         return exception;
      } else {
         console.info(`call: BadUnexpectedError ${exception.toString()}`);
         return {
            code: OpcUa.StatusCodes.BadUnexpectedError,
            message: exception.toString()
         };
      }
   }
}
