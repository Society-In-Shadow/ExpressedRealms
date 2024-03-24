import ToastEventBus from 'primevue/toasteventbus';

function success(message: string): void;
function success(title: string, message: string): void;
function success(title: string, message?: string): void {
    if(message !== undefined){
        ToastEventBus.emit("add", { severity: 'success', summary: title, detail: message, life: 3000 } )
    }else {
        ToastEventBus.emit("add", { severity: 'success', summary: "Success", detail: title, life: 3000 } )
    }
}

function error(message: string): void;
function error(title: string, message: string): void;
function error(title: string, message?: string): void {
    if(message !== undefined){
        ToastEventBus.emit("add", { severity: 'error', summary: title, detail: message, life: 3000 } )
    }else {
        ToastEventBus.emit("add", { severity: 'error', summary: "Error", detail: title, life: 3000 } )
    }
}

function info(message: string): void;
function info(title: string, message: string): void;
function info(title: string, message?: string): void {
    if(message !== undefined){
        ToastEventBus.emit("add", { severity: 'info', summary: title, detail: message, life: 3000 } )
    }else {
        ToastEventBus.emit("add", { severity: 'info', summary: "Information", detail: title, life: 3000 } )
    }
}

function warning(message: string): void;
function warning(title: string, message: string): void;
function warning(title: string, message?: string): void {
    if(message !== undefined){
        ToastEventBus.emit("add", { severity: 'warn', summary: title, detail: message, life: 3000 } )
    }else {
        ToastEventBus.emit("add", { severity: 'warn', summary: "Warning", detail: title, life: 3000 } )
    }
}

function secondary(title: string, message: string): void {
    ToastEventBus.emit("add", { severity: 'secondary', summary: title, detail: message, life: 3000 } )
}

function contrast(title: string, message: string){
    ToastEventBus.emit("add", { severity: 'contrast', summary: title, detail: message, life: 3000 });
}

export default {
    success,
    error,
    info,
    warning, 
    secondary,
    contrast
}
