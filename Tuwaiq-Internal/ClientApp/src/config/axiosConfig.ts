import axios from 'axios';
import { errorHandler } from '../utils/errorHandler';
import { BASE_URL } from './envConfig';

const myAxios = axios.create({
  baseURL: BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
});


const setupAxiosMiddleware = () => {
  const token = window.token;//(document.getElementById('token') as HTMLInputElement).value;
  myAxios.interceptors.request.use(
      (axiosRequest) => {
        axiosRequest.headers['my-x-12s7'] = token;
        return axiosRequest;
      },
      (error) => {
        return Promise.reject(error);
      }
  );
  myAxios.interceptors.response.use(
      (axiosResponse) => {
        if (axiosResponse.status >= 400) {
          throw new Error(axiosResponse.data);
        }
        return axiosResponse;
      },
      (error) => {
        if (!error.response) {
          return Promise.reject('Network Error');
        }
        errorHandler(error.response.data);
        return Promise.reject(error.response.data);
      }
  );
};

export { setupAxiosMiddleware, myAxios };
