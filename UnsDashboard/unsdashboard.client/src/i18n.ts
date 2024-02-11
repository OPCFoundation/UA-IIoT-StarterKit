import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import Backend from 'i18next-http-backend';

export const SupportedLanguages = ["en" ];

i18n
   // i18next-http-backend
   // loads translations from your server
   // https://github.com/i18next/i18next-http-backend
   .use(Backend)
   .use(initReactI18next) // passes i18n down to react-i18next
   .init({
      debug: true,
      lng: "en",
      fallbackLng: "en",
      interpolation: {
         escapeValue: false // react already safes from xss
      }
   });

export default i18n;