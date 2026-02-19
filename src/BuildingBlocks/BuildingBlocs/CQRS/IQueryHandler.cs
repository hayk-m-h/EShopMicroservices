using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocs.CQRS;

internal interface IQueryHandler<in TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull;
