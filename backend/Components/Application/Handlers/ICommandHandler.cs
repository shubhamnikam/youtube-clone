using Application.Contracts;
using MediatR;
using Application.Contracts;

namespace Application.Handlers;

/*public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
        where TCommand : ICommand
{
}
*/
public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
}