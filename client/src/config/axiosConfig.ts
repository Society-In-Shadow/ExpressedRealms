import axios from 'axios'
import toaster from "@/services/Toasters";
import Router from "@/router";

// Add a response interceptor

function setupErrorHandlingInterceptors() {
    axios.interceptors.response.use(
        (response) => response,
        (error) => {
            if (error.response) {
                if (error.response.status === 401) {
                    // Redirect to login page
                    Router.push('/login')
                } else if (error.response.status == 400) {
                    // Fluent Validation will only return 400 if it catches something, so show warning if this is received.
                    toaster.warning("Validation Errors", "One or more fields have errors on them.")
                } else {
                    // Show a generic error message
                    toaster.error('An error occurred.  Please try again.');
                }
            }
            return Promise.reject(error)
        },
    );
}

function setAPIUrl() {
    axios.defaults.baseURL = import.meta.env.VITE_API_BASE_URL
    axios.defaults.withCredentials = true;
}

export default {
    setupErrorHandlingInterceptors,
    setAPIUrl
};
