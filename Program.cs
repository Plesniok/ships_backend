using Ships.Model;
using Response.Service;
using Ships.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;




var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Description = "Keep track of your tasks", Version = "v1" });
    });

var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
    });

ShipsService ShipsServiceInstance = new ShipsService();
// app.MapGet("/", () => {
//     var response = new {
//         message = ShipsDBInstance.GetPizzas()
//     };
//     return Results.Json(response);
// });

app.MapPost("/game", (CreateGame requestBody) => {
    
    if(requestBody.ships.Count() != 10){
        return Results.Json(
            ResponseService.InfoResponse(
                "player must have 10 ships",
                "10001"
            ),
            statusCode: 400
        );
    }

    if(requestBody.ships.Count() != requestBody.ships.Distinct().Count()){
        return Results.Json(
            ResponseService.InfoResponse(
                "Player ships are not unique",
                "10001"
            ),
            statusCode: 400
        );
    }

    TableName serviceResult = ShipsServiceInstance
        .CreateGame(requestBody.playerName, requestBody.ships);
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
    
    if(requestBody.ships.Count() != 10){
        return Results.Json(
            ResponseService.InfoResponse(
                "player must have 10 ships",
                "10001"
            ),
            statusCode: 400
        );
    } 

    if(requestBody.ships.Count() != requestBody.ships.Distinct().Count()){
        return Results.Json(
            ResponseService.InfoResponse(
                "Player ships are not unique",
                "10001"
            ),
            statusCode: 400
        );
    }

    if(!ShipsServiceInstance.ifTableExist(requestBody.tableName)){
        return Results.Json(
            ResponseService.InfoResponse(
                "Game does not exist",
                "10002"
            ),
            statusCode: 400
        );
    }

    if(ShipsServiceInstance.ifTableIsLocked(requestBody.tableName)){
        return Results.Json(
            ResponseService.InfoResponse(
                "Game is locked",
                "10002"
            ),
            statusCode: 400
        );
    }

    bool serviceStatus = ShipsServiceInstance.UpdatePlayer(
        requestBody.tableName,
        requestBody.playerName,
        requestBody.ships
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

app.MapGet("/ship/get-all/available", (string tableName, string playerName) => {
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

    List<Point> ships = ShipsServiceInstance.GetPlayerAvailableShips(
        tableName,
        ifPlayerExists
    );

    return Results.Json(
        ResponseService.GetPlayerShips(
            ships
        ),
        statusCode:200
    );
});

app.MapGet("/ship/get-all/not-available", (string tableName, string playerName) => {
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

    List<Point> ships = ShipsServiceInstance.GetPlayerNotAvailableShips(
        tableName,
        ifPlayerExists
    );

    return Results.Json(
        ResponseService.GetPlayerShips(
            ships
        ),
        statusCode:200
    );
});

app.MapPut("/ship/destroy", (DestroyShip requestData) => {
    Console.WriteLine("te");
    if(!ShipsServiceInstance.ifTableExist(requestData.tableName)){
        return Results.Json(
            ResponseService.InfoResponse(
                "Game does not exist",
                "10002"
            ),
            statusCode: 400
        );
    }

    int? ifPlayerExists = ShipsServiceInstance.GetPlayerIdByName(
        requestData.tableName,
        requestData.playerName
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
    int? enemyPlayerId = ShipsServiceInstance.GetEnemyPlayer(
        ifPlayerExists
    );

    int? shipsDestroyed = ShipsServiceInstance.DestroyEnemyShip(
        requestData.tableName,
        enemyPlayerId,
        requestData.ship
    );

    if(shipsDestroyed == null){
        return Results.Json(
            ResponseService.InfoResponse(
                "Destroy enemy ship service unavailable",
                "10003"
            ),
            statusCode: 503
        );
    }
    if(shipsDestroyed == 0){
        return Results.Json(
            ResponseService.InfoResponse(
                "Ship not found",
                "10003"
            ),
            statusCode: 400
        );
    }

    return Results.Json(
        ResponseService.CreateSuccessBodyResponse(),
        statusCode:200
    );
});


app.Run();
