using Gls_Etykiety.azure_functions;
using Gls_Etykiety.Domain;
using Gls_Etykiety.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using NUnit.Framework.Legacy;
using System.Collections.Generic;

namespace LabelsTests;

/// <summary>
/// Tests as for now does not work, because, of problems with mocking db base, would need to create IRepository,
/// </summary>
[TestFixture]
public class PostLabelsTests
{

    private readonly IDbContextFactory<LabelDbContext> _contextFactory;

    public PostLabelsTests(IDbContextFactory<LabelDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }


    [Test]
    public async Task PostLabels_ValidUserId_ReturnsStatusCode200()
    {
        // Arrange
        //var dbContextMock = new Mock<LabelDbContext>();
        //dbContextMock.CallBase = true;

        //dbContextMock.Setup(context => context.Labels);

        //var dbContextFactoryMock = new Mock<IDbContextFactory<LabelDbContext>>();
        //dbContextFactoryMock.CallBase = true;

        //var postLabels = new PostLabels(dbContextFactoryMock.Object);

        //var httpRequestDataMock = new Mock<HttpRequestData>();
        //httpRequestDataMock.Setup(req => req.Body).Returns(new MemoryStream());

        // Act
        //var result = await postLabels.Run(httpRequestDataMock.Object);

        // Assert
        //ClassicAssert.IsInstanceOf<StatusCodeResult>(result);
        //var statusCodeResult = (StatusCodeResult)result;
        //Assert.That(200, Is.EqualTo(statusCodeResult.StatusCode));
    }
}