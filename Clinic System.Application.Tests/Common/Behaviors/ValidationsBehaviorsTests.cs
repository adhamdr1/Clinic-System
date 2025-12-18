namespace Clinic_System.Application.Tests.Common.Behaviors
{
    public record TestCommand : IRequest<bool>;

    public class ValidationsBehaviorsTests
    {
        //[Fact]
        //public void Method_Scenario_Outcome()
        //{
        //Arrange

        //Act

        //Assert
        //}

        [Fact]
        public async Task Handle_ValidatorsReturnErrors_ThrowsApiException()
        {
            //Arrange
            var request = new TestCommand();
            var cancellationToken = CancellationToken.None;

            // 1. محاكاة الـ RequestHandlerDelegate<TResponse> (next)
            // لن يتم استدعاء "next" أبداً في حالة الفشل، لكن يجب تعريفه.
            var nextMock = new Mock<RequestHandlerDelegate<bool>>();

            // 2. إنشاء خطأ Validation
            var validationFailure = new ValidationFailure("Property1", "Error message 1");
            var validationResult = new ValidationResult(new List<ValidationFailure> { validationFailure });

            // 3. محاكاة الـ Validators
            var validatorMock = new Mock<IValidator<TestCommand>>();
            var loggerMock = new Mock<ILogger<ValidationBehavior<TestCommand, bool>>>();

            validatorMock.Setup(v => v.ValidateAsync(
                It.IsAny<ValidationContext<TestCommand>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // 4. إنشاء الـ Behavior، وتمرير قائمة الـ Validators المحاكية
            var behavior = new ValidationBehavior<TestCommand, bool>(
                new List<IValidator<TestCommand>> { validatorMock.Object },
                loggerMock.Object
            );

            //Act
            var exception = await Assert.ThrowsAsync<ApiException>(async () =>
            {
                await behavior.Handle(request, nextMock.Object, cancellationToken);
            });

            //Assert
            Assert.Equal("Validation Failed", exception.Message);
            Assert.Contains("Error message 1", exception.Errors["Property1"]);

            // التأكد من أن الـ Handler الفعلي لم يتم استدعاؤه (next)
            nextMock.Verify(n => n(), Times.Never);
        }

        [Fact]
        public async Task Handle_NoValidationErrors_CallsNextPipelineStep()
        {
            //Arrange
            var request = new TestCommand();
            var expectedResult = true;
            var cancellationToken = CancellationToken.None;

            // 1. محاكاة الـ RequestHandlerDelegate<TResponse> (next)
            // لن يتم استدعاء "next" أبداً في حالة الفشل، لكن يجب تعريفه.
            var nextMock = new Mock<RequestHandlerDelegate<bool>>();
            nextMock.Setup(n => n()).ReturnsAsync(expectedResult);


            // 3. محاكاة الـ Validators
            var validatorMock = new Mock<IValidator<TestCommand>>();
            var loggerMock = new Mock<ILogger<ValidationBehavior<TestCommand, bool>>>();

            validatorMock.Setup(v => v.ValidateAsync(
                It.IsAny<ValidationContext<TestCommand>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // 4. إنشاء الـ Behavior، وتمرير قائمة الـ Validators المحاكية
            var behavior = new ValidationBehavior<TestCommand, bool>(
                new List<IValidator<TestCommand>> { validatorMock.Object },
                loggerMock.Object
            );

            //Act
            var result = await behavior.Handle(request, nextMock.Object, cancellationToken);

            //Assert
            Assert.Equal(expectedResult, result);

            // التأكد من أن الـ Handler الفعلي لم يتم استدعاؤه (next)
            nextMock.Verify(n => n(), Times.Once);
        }
    }
}
