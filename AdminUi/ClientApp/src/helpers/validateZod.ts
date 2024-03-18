import { AnyZodObject } from 'zod';

const validateZodInput = (schema: AnyZodObject, formData: object) => {
  const validateSchema = schema.safeParse(formData);
  if (!validateSchema.success) {
    const errorMessage = validateSchema.error.issues[0].message;
    return errorMessage;
  }
};

const validateZodForm = (schema: any, formData: object) => {
  const validateSchema = schema.safeParse(formData);
  if (!validateSchema.success) {
    return validateSchema.error.errors.reduce((acc, err) => {
      const path = err.path.join('.');
      const message = err.message;
      return { ...acc, [path]: message };
    }, {});
  }
  return null;
};

export { validateZodInput, validateZodForm };
