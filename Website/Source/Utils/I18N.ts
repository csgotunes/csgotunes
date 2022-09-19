import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';
import en from '../Translations/en.json';
import gr from '../Translations/gr.json';

void i18n
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    fallbackLng: 'en',
    interpolation: {
      escapeValue: false
    },
    resources: {}
  });

// FIXME: This is kind of a pain, can we load these dynamically instead?
i18n.addResourceBundle('en', 'translation', en);
i18n.addResourceBundle('gr', 'translation', gr);

export default i18n;
