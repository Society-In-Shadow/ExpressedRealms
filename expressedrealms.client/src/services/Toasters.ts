import ToastEventBus from 'primevue/toasteventbus';

export function popSuccess(message: string): void;
export function popSuccess(title: string, message: string): void;
export function popSuccess(title: string, message?: string): void {
    if(message !== undefined){
        ToastEventBus.emit("add", { severity: 'success', summary: title, detail: message, life: 50000 } )
    }else {
        ToastEventBus.emit("add", { severity: 'success', summary: "Success", detail: title, life: 50000 } )
    }
}

export function popError(message: string): void;
export function popError(title: string, message: string): void;
export function popError(title: string, message?: string): void {
    if(message !== undefined){
        ToastEventBus.emit("add", { severity: 'error', summary: title, detail: message, life: 3000 } )
    }else {
        ToastEventBus.emit("add", { severity: 'error', summary: "Error", detail: title, life: 3000 } )
    }
}

export function popInfo(message: string): void;
export function popInfo(title: string, message: string): void;
export function popInfo(title: string, message?: string): void {
    if(message !== undefined){
        ToastEventBus.emit("add", { severity: 'info', summary: title, detail: message, life: 3000 } )
    }else {
        ToastEventBus.emit("add", { severity: 'info', summary: "Information", detail: title, life: 3000 } )
    }
}

export function popWarning(message: string): void;
export function popWarning(title: string, message: string): void;
export function popWarning(title: string, message?: string): void {
    if(message !== undefined){
        ToastEventBus.emit("add", { severity: 'warning', summary: title, detail: message, life: 3000 } )
    }else {
        ToastEventBus.emit("add", { severity: 'warning', summary: "Warning", detail: title, life: 3000 } )
    }
}

export function popSecondary(title: string, message: string): void {
    ToastEventBus.emit("add", { severity: 'secondary', summary: "Information", detail: title, life: 3000 } )
}

export function popContrast(title: string, message: string){
    ToastEventBus.emit("add", { severity: 'contrast', summary: title, detail: message, life: 3000 });
}
