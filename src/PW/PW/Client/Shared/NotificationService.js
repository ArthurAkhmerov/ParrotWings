var PW;
(function (PW) {
    'use strict';
    var NotificationService = (function () {
        function NotificationService() {
            toastr.options.timeOut = 8000;
        }
        NotificationService.prototype.success = function (msg) {
            toastr.success(msg);
        };
        NotificationService.prototype.error = function (msg) {
            toastr.error(msg);
        };
        return NotificationService;
    })();
    angular.module("PW")
        .factory('PW.NotificationService', function () { return new NotificationService(); });
})(PW || (PW = {}));
//# sourceMappingURL=notificationService.js.map