
module PW {
    var pwApp = angular.module('PW', ['ui.bootstrap', 'ui.router', 'ngCookies', 'daterangepicker', 'ngResource']);


	pwApp.filter('time', function () {
		return function (input: moment.Moment) {
			return moment.utc(input).local().format('HH:mm');;
		}
	});

	pwApp.filter('datetime', function () {
		return function (input: moment.Moment) {
			return moment.utc(input).local().format('YYYY-MM-DD HH:mm');;
		}
	});

	pwApp.filter('timeago', function () {
		return function (input: string) {
			if (moment(input, 'YYYY-MM-DD').isSame(moment(), 'day')) {
				return "today";
			} else if (moment(input, 'YYYY-MM-DD').isSame(moment().add(-1, 'day'), 'day')) {
				return "yesterday";
			} else {
				return moment(input, 'YYYY-MM-DD').format('YYYY-MM-DD');
			}
		}
	});

	pwApp.filter('usersFilter', function ($filter) {
		return function (list: IUserVDTO[], searchText) {
			if (!searchText) return [];

			return list.filter(x => x.username.indexOf(searchText) != -1 || x.email.indexOf(searchText) != -1);
		};
	});

	pwApp.directive('validNumber', function () {
		return {
			require: '?ngModel',
			link: function (scope, element, attrs, ngModelCtrl: any) {
				if (!ngModelCtrl) {
					return;
				}

				ngModelCtrl.$parsers.push(function (val) {
					if (angular.isUndefined(val)) {
						var val: any = '';
					}

					var clean = val.replace(/[^-0-9\.]/g, '');
					var negativeCheck = clean.split('-');
					var decimalCheck = clean.split('.');
					if (!angular.isUndefined(negativeCheck[1])) {
						negativeCheck[1] = negativeCheck[1].slice(0, negativeCheck[1].length);
						clean = negativeCheck[0] + '-' + negativeCheck[1];
						if (negativeCheck[0].length > 0) {
							clean = negativeCheck[0];
						}

					}

					if (!angular.isUndefined(decimalCheck[1])) {
						decimalCheck[1] = decimalCheck[1].slice(0, 2);
						clean = decimalCheck[0] + '.' + decimalCheck[1];
					}

					if (val !== clean) {
						ngModelCtrl.$setViewValue(clean);
						ngModelCtrl.$render();
					}
					return clean;
				});

				element.bind('keypress', function (event) {
					if (event.keyCode === 32) {
						event.preventDefault();
					}
				});
			}
		}
	});

	pwApp.directive('clickAnywhereButHere', ["$document", ($document) => {
		return {
			restrict: 'A',
			link: function (scope, elem, attr: any, ctrl) {
				elem.bind('click', function (e) {
					e.stopPropagation();
				});
				$document.bind('click', function () {
					scope.$apply(attr.clickAnywhereButHere);
				})
			}
		}
	}]);
}
