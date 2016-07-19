module PW {
    'use strict';

    export interface INotificationService {
        success(msg: string);
        error(msg: string);
    }

    class NotificationService {
        private toastr: Toastr;
        constructor() {
            toastr.options.timeOut = 8000;
        }
        public success(msg: string) {
            toastr.success(msg);
        }
        public error(msg: string) {
            toastr.error(msg);

        }
    }

    angular.module("PW")
        .factory('PW.NotificationService', () => new NotificationService());
}