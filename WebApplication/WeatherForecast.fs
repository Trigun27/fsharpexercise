namespace WebApplication

open System

type Animal = {
    Name: string
    Species: string
}



type WeatherForecast =
    { Date: DateTime
      TemperatureC: int
      Summary: string
      Test: string option}

    member this.TemperatureF =
        32.0 + (float this.TemperatureC / 0.5556)
