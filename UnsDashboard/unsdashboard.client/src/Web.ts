/* eslint-disable @typescript-eslint/no-explicit-any */

import { parseISO, parse, formatISO, format } from 'date-fns'

export type Color = 'inherit' | 'default' | 'primary' | 'secondary' | 'error' | 'info' | 'success' | 'warning';

export function utcNow() {
   return formatISO(new Date(), { representation: 'complete' });
}

export function formatDate(input?: string) {
   return (input) ? formatISO(parseISO(input), { representation: 'date' }) : null;
}

export function formatDateTime(input?: string) {
   if (!input) return null;
   const date = parseISO(input);
   date.toISOString()
   const newDate = new Date(date.getTime() + date.getTimezoneOffset() * 60 * 1000);
   const offset = date.getTimezoneOffset() / 60;
   const hours = date.getHours();
   newDate.setHours(hours - offset);
   return format(newDate, 'yyyy-MM-dd HH:mm:ss');
}

export function dateToLocalTime(input?: Date) {
   if (input instanceof Date) {
      const newDate = new Date(input.getTime() + input.getTimezoneOffset() * 60 * 1000);
      const offset = input.getTimezoneOffset() / 60;
      const hours = input.getHours();
      newDate.setHours(hours - offset);
      return format(input, 'HH:mm:ss');
   }
   if (input) {
      return formatTime(input);
   }
   return '';
}

export function formatTime(input?: string) {
   return formatDateTime(input)?.substring(11);
}

export function formatDateString(input?: string) {
   return (input) ? formatISO(parse(input, "yyyyMMddHHmmss", new Date()), { representation: 'date' }) : null;
}

export async function readResponseBody(url: string, response: any) {
   const content = response.headers.get("Content-Type");

   if (content && content.indexOf("json") < 0) {
      console.error("URL: " + url);
      console.error("UnexpectedResponse: " + await response.text());
      return null;
   }

   return await response.json();
}

export async function httpGet(url: string, controller: AbortController) {
   try {
      console.info(`httpGet ${url}`);
      const response = await fetch(url, (controller) ? { signal: controller.signal } : {});
      if (response.redirected) {
         console.info(`httpRedirect: ${response.url}`);
         window.location.href = response.url;
      }
      else if (response.ok) {
         const body = await readResponseBody(url, response);
         if (body && !body.failed) {
            return {
               ...body,
               errorText: (body.errorCode) ? body.errorText ?? `${body.errorCode}Text` : null
            };
         }
         else {
            console.info(`httpGet: ${body?.errorCode} ${body?.errorText}`);
            return {
               failed: body?.failed ?? true,
               errorCode: body?.errorCode,
               errorText: body?.errorText
            }
         }
      }
      else {
         console.info(`httpGet: ${response.status} ${response.statusText}`);
         return {
            failed: true,
            errorCode: `HTTP ${response.status}`,
            errorText: response.statusText
         }
      }
   }
   catch (exception: any) {
      if (exception.code) {
         console.info(`httpGet: ${exception.code} ${exception.message}`);
         return {
            failed: true,
            errorCode: exception.code,
            errorText: exception.message,
            silent: exception.code === 20 // suppress aborts.
         }
      } else {
         console.info(`httpGet: UnexpectedError ${exception}`);
         return {
            failed: true,
            errorCode: 'UnexpectedError',
            errorText: exception.toString()
         }
      }
   }
}

export async function httpPost(url: string, item: any, controller: AbortController) {
   try {
      const requestOptions = {
         method: 'POST',
         headers: { 'Content-Type': 'application/json' },
         body: JSON.stringify((item.toJson) ? item.toJson(item) : item),
         signal: controller?.signal
      };

      console.info(`httpPost: ${url}`);
      // console.info("BODY: " + requestOptions.body);
      const response = await fetch(url, requestOptions);
      if (response.redirected) {
         console.info(`httpRedirect: ${response.url}`);
         window.location.href = response.url;
      }
      else if (response.ok) {
         const body = await readResponseBody(url, response);
         if (body && !body.failed) {
            return body;
         }
         else {
            console.info(`httpPost: ${body?.errorCode} ${body?.errorText}`);
            return {
               failed: true,
               errorCode: body?.errorCode,
               errorText: body?.errorText
            }
         }
      }
      else {
         console.info(`httpPost: ${response.status} ${response.statusText}`);
         return {
            failed: true,
            errorCode: `HTTP ${response.status}`,
            errorText: response.statusText
         }
      }
   }
   catch (exception: any) {
      if (exception.code) {
         console.info(`httpPost: ${exception.code} ${exception.message}`);
         return {
            failed: true,
            errorCode: exception.code,
            errorText: exception.message,
            silent: exception.code === 20 // suppress aborts.
         }
      } else {
         console.info(`httpPost: UnexpectedError ${exception}`);
         return {
            failed: true,
            errorCode: 'UnexpectedError',
            errorText: exception.toString()
         }
      }
   }

   return item;
}

export function extractFileName(response: Response): string | null {
   const header = response.headers.get('Content-Disposition');
   if (!header) {
      return null;
   }
   const regex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
   const match = regex.exec(header);
   if (match && match[1]) {
      return match[1].replace(/['"]/g, '');
   }
   return null;
}

export async function saveFile(fileName: string | null, blob: Blob) {
   // Create a download link
   const url = window.URL.createObjectURL(blob);

   // Create an anchor element
   const link = document.createElement('a');
   link.href = url;

   // Set the filename
   link.setAttribute('download', fileName ?? 'downloaded_file.txt');

   // Simulate a click to trigger the download
   document.body.appendChild(link);
   link.click();

   // Clean up the temporary object URL
   URL.revokeObjectURL(url);
   document.body.removeChild(link);
}
