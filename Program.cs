using Ships.Model;
using Response.Service;
using Ships.Service;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
ShipsService ShipsServiceInstance = new ShipsService();
// app.MapGet("/", () => {
//     var response = new {
//         message = ShipsDBInstance.GetPizzas()
//     };
//     return Results.Json(response);
// });

app.MapPost("/game", (CreateGame requestBody) => {
    
    TableName serviceResult = ShipsServiceInstance
        .CreateGame(requestBody.playerName);
    if(serviceResult == new TableName()){
        return Results.Json(
            ResponseService.InfoResponse(
                "Create game service unavailable",
                "10001"
            ),
            statusCode: 503
        );
    }
    return Results.Json(
        ResponseService.CreateGameResponse(
            serviceResult.tableName
        ),
        statusCode:200
    );
});

app.MapPut("/player/two", (AddPlayer requestBody) => {
    
    if(!ShipsServiceInstance.ifTableExist(requestBody.tableName)){
        return Results.Json(
            ResponseService.InfoResponse(
                "Game does not exist",
                "10002"
            ),
            statusCode: 400
        );
    }

    bool serviceStatus = ShipsServiceInstance.UpdatePlayer(
        requestBody.tableName,
        requestBody.playerName
    );
    if(!serviceStatus){
        return Results.Json(
            ResponseService.InfoResponse(
                "Add 2nd player service unavailable",
                "10003"
            ),
            statusCode: 503
        );
    }
    return Results.Json(
        ResponseService.CreateSuccessBodyResponse(),
        statusCode:200
    );
});



app.MapGet("/player/index", (string tableName, string playerName) => {
    if(!ShipsServiceInstance.ifTableExist(tableName)){
        return Results.Json(
            ResponseService.InfoResponse(
                "Game does not exist",
                "10002"
            ),
            statusCode: 400
        );
    }

    int? ifPlayerExists = ShipsServiceInstance.GetPlayerIdByName(
        tableName,
        playerName
    );

    if(ifPlayerExists == null){
        return Results.Json(
            ResponseService.InfoResponse(
                "Get player id by name service unavailable",
                "10003"
            ),
            statusCode: 503
        );
    }

    if(ifPlayerExists == 0){
        return Results.Json(
            ResponseService.InfoResponse(
                "Player does not exist in given Game",
                "10002"
            ),
            statusCode: 400
        );
    }

    

    return Results.Json(
        ResponseService.CreateGetPlayerByNameResponse(
            ifPlayerExists
        ),
        statusCode:200
    );
});


app.Run();
