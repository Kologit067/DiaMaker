

var dataBaseApp = angular.module("DataBaseApp");

dataBaseApp.controller("GetDataBasesList", function GetDataBasesList($scope, $http) {
    $scope.model = {
        databases: null,
        selectedDataBase: null,
        tables: null,
        errorText: null
    };

    $http.get('../api/database').success(function (data) {
        $scope.model.databases = data;
        $scope.model.selectedDataBase = data[0].Name;
        GetTables();
    });

    $scope.selectedDataBaseChanged = function () {

        GetTables();
       
    };

    function GetTables() {
        var uri = '../api/tables/' + $scope.model.selectedDataBase;
        $http({
            method: 'GET',
            url: uri
        }).then(function successCallback(response) {
            $scope.model.tables = response.data;
        }, function errorCallback(response) {
            $scope.model.errorText = response.statusText;
        });
    };

    $scope.SelectAll = function () {
        for (var index in $scope.model.tables) {

            $scope.model.tables[index].IsSelected = true;
        }
    }

    $scope.creatDia = function () {
        var uri = '../api/tables/' + $scope.model.selectedDataBase;
        $http({
            method: 'POST',
            url: uri,
            data: $scope.model.tables  
        }).then(function successCallback(response) {
            var canvas = document.getElementById("diaCanvas"),
            context = canvas.getContext("2d");
            context.clearRect(0, 0, canvas.width, canvas.height);
            context.strokeStyle = "#0000FF"
            context.font = "22px Verdana";
            var rectangles = response.data.Rectangles;
            for (var index in rectangles)
            {
                context.strokeRect(rectangles[index].Left, rectangles[index].Top, rectangles[index].Width, rectangles[index].Height);
                context.fillStyle = "#999999";
                context.fillRect(rectangles[index].Left, rectangles[index].Top, rectangles[index].Width, rectangles[index].Height);
                context.fillStyle = "#000000";
                context.fillText(rectangles[index].Name, rectangles[index].Left + 5, rectangles[index].Top + 30);
            }

            var connectLines = response.data.ConnectLines;
            for (var index in connectLines) {
                var points = connectLines[index].Points;
                context.beginPath();
                context.moveTo(points[0].X, points[0].Y);
                for (var i = 1; i < points.length; i++)
                {
                    context.lineTo(points[i].X, points[i].Y);
                }
                context.strokeStyle = "#0000FF";
                context.stroke();
            }

            var arrows = response.data.Arrows;
            for (var index in arrows) {
                var points = arrows[index].Points;
                context.beginPath();
                context.moveTo(points[0].X, points[0].Y);
                for (var i = 1; i < points.length; i++) {
                    context.lineTo(points[i].X, points[i].Y);
                }
                context.closePath();
                context.strokeStyle = "#0000AA";
                context.stroke();
            }

        }, function errorCallback(response) {
            $scope.model.errorText = response.statusText;
        });


    //    $http.post("../api/tables/", $scope.model.tables).success(function (dia) {

    //        var canvas = document.getElementById("myCanvas"),
    //context = canvas.getContext("2d");

    //        context.strokeRect(50, 40, 100, 100);
    //        context.fillRect(200, 40, 100, 100);
    //    });
    };
})