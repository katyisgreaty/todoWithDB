using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
  public class ToDoTest : IDisposable
  {
    public ToDoTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Task.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_EqualOverrideTrueForSameDescription()
    {
      //Arrange, Act
      Task firstTask = new Task("Mow the lawn", 1, "2017-03-02");
      Task secondTask = new Task("Mow the lawn", 1, "2017-03-02");

      //Assert
      Assert.Equal(firstTask, secondTask);
    }

    [Fact]
    public void Test_Save()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", 1, "2017-03-02");
      testTask.Save();

      //Act
      List<Task> result = Task.GetAll();
      List<Task> testList = new List<Task>{testTask};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_SaveAssignsIdToObject()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", 1, "2017-03-02");
      testTask.Save();

      //Act
      Task savedTask = Task.GetAll()[0];

      int result = savedTask.GetId();
      int testId = testTask.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsTaskInDatabase()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", 1, "2017-03-02");
      testTask.Save();

      //Act
      Task foundTask = Task.Find(testTask.GetId());

      //Assert
      Assert.Equal(testTask, foundTask);
    }

    [Fact]
    public void Test_DueDate_GetDueDate()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", 1, "2017-03-02");
      testTask.Save();
      string expected = ("2017-03-02");

      //Act
      string foundDueDate = testTask.GetDueDate();

      //Assert
      Assert.Equal(expected, foundDueDate);
    }

    [Fact]
    public void Test_DueDate_GetSortedList()
    {
      //Arrange
      Task firstTask = new Task("Mow the lawn", 1, "2017-03-02");
      Task secondTask = new Task("Walk the dog", 1, "2017-05-02");
      Task thirdTask = new Task("Wash dishes", 1, "2017-03-12");
      firstTask.Save();
      secondTask.Save();
      thirdTask.Save();


      //Act
      List<Task> allTaskList = new List<Task>{firstTask, secondTask, thirdTask};
      foreach (Task task in allTaskList)
      {
          Console.WriteLine(task.GetDescription());
      }
      List<Task> expectedList = new List<Task>{firstTask, thirdTask, secondTask};
      List<Task> testList = Task.GetSortedList();

      //Assert
      Assert.Equal(testList, expectedList);
    }

    public void Dispose()
    {
      Task.DeleteAll();
    }
  }
}
