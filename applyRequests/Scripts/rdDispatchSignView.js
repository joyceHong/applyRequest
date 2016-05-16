angular.module("requestApp", ["kendo.directives"])
     .directive("signViewTemplate", function () {
         return {
             templateUrl: 'signViewTemplate'
         }
     }).service('getTaskUsers', function ($http) {
         this.getRdUsers = function () {
             return $http.get("../api/UserAPI?userListType=rdUsers&userID=''").then(function (result) {
                 listRdusers = result.data;
                 return listRdusers;
             });
         }
     }).factory('myService', function ($http) {
         var listRdusers;
         return {
             getFoo: function () {
                 //return $http.get("/api/UserAPI?userListType=rdUsers&userID=''");
                 return $http.get("../api/UserAPI?userListType=rdUsers&userID=''").then(function ( result) {
                     listRdusers = result.data;
                     return listRdusers;
                 });
             }
         }
     })
    .controller("rdDispatchSignViewController", function ($scope, $http) {

        $scope.monthPicker = {
            start: "day",
            depth: "day",
            format: "yyyy-MM-dd"
        };

        $(document).ready(function () {
            $.ajax({
                url: "../api/UserAPI?userListType=rdUsers&userID=''",
                type: "Get",
                success: function (data) {
                    var mySelect = $('#taskUser');
                    $.each(data, function (i, item) {
                        mySelect.append(
                            $('<option></option>').val(item.strRoleUserID).html(item.strRoleUserName)
                        );
                    });
                },
                error: function (msg) { alert(msg); }
            });
        });

        //$scope.taskusers = [
        //       { strRoleUserID: "", strRoleUserName: '請選擇', title: 1 },
        //       { strRoleUserID: "0009", strRoleUserName: '許至孝', title: 1 },
        //       { strRoleUserID: "0019", strRoleUserName: '蘇志孟', title: 2 },               
        //       { strRoleUserID: "0027", strRoleUserName: '林志麟', title: 3 },
        //       { strRoleUserID: "0137", strRoleUserName: '洪秋雲', title: 3 }
        //];

        //$scope.taskusers = [];
        //myService.getFoo().success(function (response) {
        //    console.log(response.data);
        //    $scope.users = response.data;
        //}).error(function (err) {
        //    alert('Error: ' + err);
        //});

        //$scope.taskusers = [];
        //myService.getFoo().then(function (response) {
        //    console.log(response);
        //    $scope.taskusers = response;
        //})


        //getTaskUsers.getRdUsers().then(function (response) {
        //    console.log(response);
        //    $scope.taskusers = response;
        //});
    });