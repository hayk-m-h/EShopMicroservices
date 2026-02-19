using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocs.CQRS;

internal interface IQuery<out TResponse> : IRequest<TResponse>
    where TResponse : notnull;
