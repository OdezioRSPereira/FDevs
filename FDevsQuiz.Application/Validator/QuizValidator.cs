using FDevsQuiz.Domain.Command;
using FluentValidation;

namespace FDevsQuiz.Application.Validator
{
    public class QuizValidator: AbstractValidator<AddQuizCommand>
    {

        public QuizValidator()
        {
            RuleFor(x => x.Titulo).
                NotEmpty().WithMessage("Titulo do quiz é obrigatório.").
                Length(10, 50).WithMessage("Titulo deve contem de {MinLength} a {MaxLength} letras.");
           // RuleForEach(x => x.Perguntas).SetValidator(new PerguntaValidator());
        }
    }
}
