namespace WebApplication.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Net
open System.Net.Http
open System.Net.Http
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open WebApplication


//module Helpers =
//    let asResponse (request: HttpRequestMessage) result =
//        match result with
//        | Some result ->
//        | None -> 


[<ApiController>]
[<Route("[controller]")>]
type WeatherForecastController (logger : ILogger<WeatherForecastController>) =
    inherit ControllerBase()
