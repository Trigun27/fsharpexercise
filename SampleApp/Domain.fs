module Domain

open System

type Currency =
    | RUB
    | DOLLAR
    | EURO

type Customer = {
    Name: string
}

type Account = {
    Id: Guid
    Currency: Currency
    Amount: decimal
    Customer: Customer
}

