using HelpDesk.Domain.Entities;

namespace HelpDesk.UnitTests.UnitTests.Domain;

[TestClass]
public sealed class SupportTeamBehaviorTests
{
    private static SupportTeam CreateValidSupportTeam(string name = "test")
    {
        return new SupportTeam(name);
    }


    [TestMethod]
    public void Constructor_WithValidData_CreatesSupportTeam()
    {
        var supportTeam = CreateValidSupportTeam();

        Assert.IsNotNull(supportTeam);
        Assert.AreNotEqual(Guid.Empty, supportTeam.Id);
        Assert.AreEqual("test", supportTeam.Name);
    }


    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    public void Constructor_WithInvalidData_ThrowsArgumentException(string teamName)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new SupportTeam(teamName));

        Assert.IsNotNull(exception);
        Assert.AreEqual("name", exception.ParamName);
    }


    [TestMethod]
    public void Rename_WithValidName_RenamesTeams()
    {
        var supportTeam = CreateValidSupportTeam();

        supportTeam.Rename("Test 2");

        Assert.AreEqual("Test 2", supportTeam.Name);
    }


    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    public void Rename_WithInvalidName_ThrowsArgumentException(string teamName)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            CreateValidSupportTeam().Rename(teamName));

        Assert.AreEqual("name", exception.ParamName);
    }


    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    public void Rename_WithInvalidName_DoesnChangeName(string invalidName)
    {
        var supportTeam = CreateValidSupportTeam();

        Assert.Throws<ArgumentException>(() => supportTeam.Rename(invalidName));

        Assert.AreEqual("test", supportTeam.Name);
    }
}
