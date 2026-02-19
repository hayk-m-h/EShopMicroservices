using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocs.CQRS;

public interface ICommand : IRequest<Unit>;

public interface ICommand<out TResponse> : IRequest<TResponse>;
