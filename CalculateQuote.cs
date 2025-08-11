private decimal CalculateQuote(Insuree insuree)
{
    decimal quote = 50m;

    int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
    if (insuree.DateOfBirth > DateTime.Now.AddYears(-age)) age--;

    if (age <= 18)
        quote += 100;
    else if (age >= 19 && age <= 25)
        quote += 50;
    else
        quote += 25;

    if (insuree.CarYear < 2000)
        quote += 25;
    if (insuree.CarYear > 2015)
        quote += 25;

    if (insuree.CarMake.ToLower() == "porsche")
    {
        quote += 25;
        if (insuree.CarModel.ToLower() == "911 carrera")
            quote += 25;
    }

    quote += insuree.SpeedingTickets * 10;

    if (insuree.DUI)
        quote *= 1.25m;

    if (insuree.CoverageType)
        quote *= 1.50m;

    return quote;
}
