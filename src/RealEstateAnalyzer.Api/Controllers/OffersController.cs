using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstateAnalyzer.Application.Offers.Commands.CreateOffer;
using RealEstateAnalyzer.Application.Offers.Commands.SetRentalAssumptions;
using RealEstateAnalyzer.Application.Offers.Dtos;
using RealEstateAnalyzer.Application.Offers.Queries.GetOfferById;

namespace RealEstateAnalyzer.Api.Controllers;

[ApiController]
[Route("api/offers")]
public sealed class OffersController(ISender sender) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreatedOfferResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateOfferCommand command, CancellationToken cancellationToken)
    {
        var id = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new CreatedOfferResponse(id));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OfferDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var offer = await sender.Send(new GetOfferByIdQuery(id), cancellationToken);
        return offer is null ? NotFound() : Ok(offer);
    }

    [HttpPut("{id:guid}/rental-assumptions")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetRentalAssumptions(
        Guid id,
        SetRentalAssumptionsRequest request,
        CancellationToken cancellationToken)
    {
        var found = await sender.Send(
            new SetRentalAssumptionsCommand(id, request.MonthlyRent, request.VacancyMonthsPerYear, request.OtherMonthlyCosts),
            cancellationToken);

        return found ? NoContent() : NotFound();
    }

    public sealed record CreatedOfferResponse(Guid Id);

    public sealed record SetRentalAssumptionsRequest(
        decimal MonthlyRent,
        int VacancyMonthsPerYear = 0,
        decimal OtherMonthlyCosts = 0);
}
