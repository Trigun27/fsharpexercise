open System
open System.Globalization
open System.IO
open System.Text.RegularExpressions

type Photo = {
    YearFolder: string
    MonthFolder: string
    Path: string
    Name: string
}

let ios = "^\d*_\d*_iOS"
let wp = "^WP_\d*_\d*"
//let path = @"E:\OneDrive\Изображения\Test\"

let path = @"E:\OneDrive\Изображения\Пленка\"


let photos =
        Directory.EnumerateFiles(path)
        |> Seq.map (fun p -> p.Replace(path, ""))
        |> Seq.toList
let splitName (name: string) =    
    let photoData = name.Replace("WP_", "").Split("_")    
    let date = DateTime.ParseExact(photoData.[0], "yyyyMMdd", CultureInfo.InvariantCulture)
    let year = date.Year.ToString()
    let month = date.Month.ToString "d2"   
    {
        YearFolder = year
        MonthFolder = month
        Path = Path.Combine(path, year, month)
        Name = name
    }
      
let movePhotos (photos: Photo list) =
    let checkPath photo =
        let directory = Directory.CreateDirectory(photo.Path)
        let fullPath = Path.Combine(photo.Path, photo.Name)
        not(File.Exists(fullPath))
    
    let copyPhoto photo =
        let fullPath = Path.Combine(photo.Path, photo.Name)    
        File.Copy( Path.Combine(path, photo.Name), fullPath, false)
         
    photos
    |> List.filter checkPath
    |> List.iter copyPhoto
    
let takePhotoNames (reg: Regex) =             
    photos
    |> List.filter reg.IsMatch
    |> List.map splitName

let start =
    let iosReg = Regex ios
    let wpReg = Regex wp
    
    let iosPhotos =
        iosReg
        |> takePhotoNames
    
    let wpPhotos =
        wpReg
        |> takePhotoNames
    
    iosPhotos @ wpPhotos
    |> movePhotos
    
[<EntryPoint>]
let main argv =
    

    0 // return an integer exit code