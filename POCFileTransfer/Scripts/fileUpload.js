//inject angular file upload directives and services.
var app = angular.module('fileUpload', ['ngFileUpload', 'treeControl']);

app.controller('MyCtrl', ['$scope', 'Upload','$http', function ($scope, Upload, $http) {
    $scope.treeOptions = {
        nodeChildren: "FilesinFolder",
        dirSelectable: true,
        injectClasses: {
            ul: "a1",
            li: "a2",
            liSelected: "a7",
            iExpanded: "a3",
            iCollapsed: "a4",
            iLeaf: "a5",
            label: "a6",
            labelSelected: "a8"
        }
    }

    $scope.files = [];
    $scope.log = '';
    $scope.$watch('files', function () {
            $scope.upload($scope.files);
        
    });
    $scope.namer = "";
    $scope.showSelected = function (node, selected) {
        if (selected) {
            $scope.namer = node.path;
        }
    }
    $scope.getserver = function () {
        // Simple POST request example (passing data) :
        $http.post('/api/tree/download', { "Url": "" }).
          success(function (data, status, headers, config) {
              // this callback will be called asynchronously
              // when the response is available
              $scope.dataForTheTree = JSON.parse('{"FilesinFolder":['+JSON.stringify(data)+'], "path":null, "ObjectName":"root"}');
              $scope.expandedNodes = [$scope.dataForTheTree.FilesinFolder[0]];
              $scope.log += JSON.stringify(data);
          }).
          error(function (data, status, headers, config) {
              $scope.log += "error getting file structure";
              // called asynchronously if an error occurs
              // or server returns response with an error status.
          });
    }

    $scope.upload = function (files) {
        if (files && files.length) {
            for (var i = 0; i < files.length; i++) {
                var file = files[i];

                if (file.name=="desktop.ini") {
                    console.log("desktop.ini processed but not uploaded");
                } else {
                    if (file.path == null) {
                        //path is undefined on single file upload, so default to file name
                        file.path = file.name;
                        console.log("file path is undefined, set to file name");
                    }
                        Upload.upload({
                            url: '/api/file/upload/',

                            
                            fileName: $scope.namer+file.path,


                            file: file

                        }).progress(function (evt) {
                            var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                            $scope.log = 'progress: ' + progressPercentage + '% ' +
                                        evt.config.file.path + '\n' + $scope.log;
                        }).success(function (data, status, headers, config) {
                            $scope.log = 'file ' + $scope.namer+file.path + ' uploaded. Response: ' + JSON.stringify(data) + '\n' + $scope.log;
                            $scope.getserver();
                            //line causes error
                            // $scope.$apply();

                        });
                }
            }
        }
    };
}]);
