import { toastError } from "../config/toastifyConfig";
import { clientErrors, serverErrors } from "./errorMesasge";


const errorHandler = (error) => {
  if (error.status == 500) {
    toastError(clientErrors.serverError);
    return;
  }
  if (error.status == 400 && error.title == serverErrors.validation) {
    const firstKey = Object.keys(error.errors)[0];
    const firstError = error.errors[firstKey][0];
    toastError(firstError);
    return;
  }
  if (error.errorMessage) {
    toastError(error.errorMessage);
    return;
  }
  toastError(clientErrors.serverError);

}



export { errorHandler };
