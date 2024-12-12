using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using BPR2_Desktop.Views.Components;
using BPR2_Desktop.Views.Pages;
using BPR2_Desktop.Views.Windows;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

public class DesignEditorTests
{
    private readonly DesignEditor _designEditor;
    private readonly Mock<DesignCanvasControl> _mockCanvasControl;

    public DesignEditorTests()
    {
        var realCanvasControl = new DesignCanvasControl(); // Use a real instance
        _designEditor = new DesignEditor
        {
            DesignCanvasControl = realCanvasControl
        };
    }

    [StaFact]
    public void Constructor_InitializesEventHandlers()
    {
        Assert.NotNull(_designEditor);
    }

    [StaFact]
    public void UpdateDimensions_StoresCorrectValues()
    {
        // Act
        _designEditor.UpdateDimensions(10, 20, 30);

        // Assert
        var (width, length, height) = _designEditor.GetDimensions();
        Assert.Equal(10, width);
        Assert.Equal(20, length);
        Assert.Equal(30, height);
    }

    [StaFact]
    public void GetDimensions_ReturnsCorrectValues()
    {
        // Arrange
        _designEditor.UpdateDimensions(15, 25, 35);

        // Act
        var (width, length, height) = _designEditor.GetDimensions();

        // Assert
        Assert.Equal(15, width);
        Assert.Equal(25, length);
        Assert.Equal(35, height);
    }
    
    /*[StaFact]
    public void Save_Click_WithNoDesignFile_PromptsForName()
    {
        // Arrange
        AppState.Instance.CurrentDesignFile = null; // Ensure no design is loaded

        var inputDialogMock = new Mock<InputDialog>("Enter the name for the new design:");
        inputDialogMock.Setup(d => d.ShowDialog()).Returns(true);
        inputDialogMock.SetupGet(d => d.ResponseText).Returns("NewDesign");

        // Replace InputDialog creation with the mock
        var designEditor = new DesignEditor
        {
            DesignCanvasControl = new DesignCanvasControl()
        };

        // Act
        designEditor.Save_Click(null, null);

        // Assert
        Assert.NotNull(AppState.Instance.CurrentDesignFile);
        Assert.EndsWith("NewDesign.json", AppState.Instance.CurrentDesignFile);
    }*/
    
    /*[StaFact]
    public void LoadDesign_Click_WithValidFile_LoadsDesign()
    {
        // Arrange
        var tempFilePath = "testDesign.json";
        File.WriteAllText(tempFilePath, JsonSerializer.Serialize(new
        {
            dimensions = new { X = 20, Z = 30, Y = 10 },
            ElementPositions = new[]
            {
                new { ElementName = "Square1", X = 10.0, Z = 20.0, Rotation = 45.0 }
            }
        }));

        var designCanvas = new DesignCanvasControl();
        _designEditor.DesignCanvasControl = designCanvas;

        //print out the design canvas values
        Console.WriteLine(designCanvas.Width);
        Console.WriteLine(designCanvas.Height);
        // Act
        _designEditor.LoadDesign_Click(null, null);

        // Assert
        Assert.Equal(20, designCanvas.Width);
        Assert.Equal(30, designCanvas.Height);

        // Cleanup
        File.Delete(tempFilePath);
    }*/
    
    /*[StaFact]
    public void SetDimensions_Click_ShowsDialog()
    {
        // Arrange
        var setDimensionsMock = new Mock<SetDimensions>(_designEditor.DesignCanvasControl, _designEditor);
        setDimensionsMock.Setup(d => d.ShowDialog()).Returns(true);

        // Act
        _designEditor.SetDimensions_Click(null, null);

        // Assert
        // Ensure the SetDimensions dialog was shown (mock ShowDialog method)
        setDimensionsMock.Verify(d => d.ShowDialog(), Times.Once);
    }*/
    
    [StaFact]
    public void UpdateDimensions_And_GetDimensions_WorkTogether()
    {
        // Act
        _designEditor.UpdateDimensions(12.5, 15.5, 18.5);

        // Assert
        var (width, length, height) = _designEditor.GetDimensions();
        Assert.Equal(12.5, width);
        Assert.Equal(15.5, length);
        Assert.Equal(18.5, height);
    }
    
    /*[Fact]
    public void Save_Click_WhenNoCurrentDesignFile_PromptsForDesignNameAndSavesFile()
    {
        // Arrange
        AppState.Instance.CurrentDesignFile = null;

        // Use a real DesignCanvasControl
        var realCanvasControl = new DesignCanvasControl();
        var designEditor = new DesignEditor
        {
            DesignCanvasControl = realCanvasControl
        };

        // Mock user interaction in InputDialog
        var inputDialog = new InputDialog("Enter the name for the new design:");
        typeof(InputDialog).GetProperty("ResponseText")?.SetValue(inputDialog, "TestDesign");

        // Temporarily override ShowDialog behavior
        inputDialog.ShowDialog = () => true;

        // Act
        designEditor.Save_Click(null, null);

        // Assert
        Assert.NotNull(AppState.Instance.CurrentDesignFile);
        Assert.EndsWith("TestDesign.json", AppState.Instance.CurrentDesignFile);
    }*/

    
    /*
    [WpfFact]
    public void Save_Click_WhenCurrentDesignFileExists_SavesToExistingFile()
    {
        // Arrange
        string projectDirectory = Directory.GetCurrentDirectory();
        string dataDirectory = Path.Combine(projectDirectory, "Data");
        string testFilePath = Path.Combine(dataDirectory, "existingDesign.json");

        // Ensure `Data` folder exists
        if (!Directory.Exists(dataDirectory))
        {
            Directory.CreateDirectory(dataDirectory);
        }

        // Create a design file
        File.WriteAllText(testFilePath, JsonSerializer.Serialize(new
        {
            dimensions = new { X = 20, Z = 30, Y = 10 },
            ElementPositions = new List<object>
            {
                new { ElementName = "Square1", X = 100, Z = 200, Rotation = 45 }
            }
        }));

        AppState.Instance.CurrentDesignFile = testFilePath;

        // Use a real instance of DesignCanvasControl
        var realCanvasControl = new DesignCanvasControl();

        // Simulate adding elements to the canvas
        Application.Current.Dispatcher.Invoke(() =>
        {
            var element = new FrameworkElement(); // Replace with specific UIElement type if needed
            realCanvasControl.AddChild(element);
        });

        var designEditor = new DesignEditor
        {
            DesignCanvasControl = realCanvasControl
        };

        // Act
        Application.Current.Dispatcher.Invoke(() =>
        {
            designEditor.Save_Click(null, null);
        });

        // Assert
        string savedContent = File.ReadAllText(testFilePath);
        Assert.Contains("Square1", savedContent); // Validate the JSON content

        // Cleanup
        if (File.Exists(testFilePath)) File.Delete(testFilePath);
        if (Directory.Exists(dataDirectory)) Directory.Delete(dataDirectory);
    }
    */


    
    [StaFact]
    public void LoadDesign_Click_WithValidFile_LoadsDesignToCanvas()
    {
        // Arrange
        string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        string dataDirectory = Path.Combine(projectDirectory, "Data");

        // Ensure the `Data` folder exists
        if (!Directory.Exists(dataDirectory))
        {
            Directory.CreateDirectory(dataDirectory); // Create the folder
        }

        string testFilePath = Path.Combine(dataDirectory, "testDesign.json");
        File.WriteAllText(testFilePath, JsonSerializer.Serialize(new
        {
            dimensions = new { X = 20, Z = 30, Y = 10 },
            ElementPositions = new List<object>
            {
                new { ElementName = "Square1", X = 100, Z = 200, Rotation = 45 }
            }
        }));

        var realCanvasControl = new DesignCanvasControl();
        var designEditor = new DesignEditor
        {
            DesignCanvasControl = realCanvasControl
        };

        // Act
        designEditor.LoadDesign_Click(null, null);

        // Assert
        var (width, length, height) = designEditor.GetDimensions();
        Assert.Equal(20, width);
        Assert.Equal(30, length);
        Assert.Equal(10, height);

        // Cleanup
        File.Delete(testFilePath);
        Directory.Delete(dataDirectory, true);
    }




    
    [StaFact]
    public void LoadDesign_Click_WhenDataFolderMissing_ShowsErrorMessage()
    {
        // Arrange
        var designEditor = new DesignEditor();

        // Act
        designEditor.LoadDesign_Click(null, null);

        // Assert
        // Validate error message logic (could use a mockable MessageBox abstraction)
    }
    
    [StaFact]
    public void SetDimensions_Click_ShowsAndClosesSetDimensionsDialog()
    {
        // Arrange
        var realCanvasControl = new DesignCanvasControl();
        var designEditor = new DesignEditor
        {
            DesignCanvasControl = realCanvasControl
        };

        // Manually create the SetDimensions dialog
        var setDimensionsDialog = new SetDimensions(realCanvasControl, designEditor);

        // Use Dispatcher to close the dialog immediately after it is shown
        setDimensionsDialog.Loaded += (sender, args) =>
        {
            // Schedule the Close call on the Dispatcher to ensure it happens after the dialog is shown
            setDimensionsDialog.Dispatcher.InvokeAsync(() => setDimensionsDialog.Close());
        };

        // Act
        setDimensionsDialog.ShowDialog();

        // Assert
        Assert.False(setDimensionsDialog.IsVisible, "The SetDimensions dialog should be closed.");
    }




    
    /*[StaFact]
    public void SaveDesignToFile_SerializesAndSavesCorrectly()
    {
        // Arrange
        string testFilePath = "testDesign.json";
        var designCanvasMock = new Mock<DesignCanvasControl>();
        designCanvasMock.Setup(c => c.GetElementPositions()).Returns(new List<DesignCanvasControl.ElementPosition>
        {
            new DesignCanvasControl.ElementPosition("Square1") { X = 100, Z = 200, Rotation = 45 }
        });

        var designEditor = new DesignEditor
        {
            DesignCanvasControl = designCanvasMock.Object
        };
        designEditor.UpdateDimensions(10, 20, 30);

        // Act
        designEditor.SaveDesignToFile(testFilePath, designCanvasMock.Object);

        // Assert
        var jsonContent = File.ReadAllText(testFilePath);
        Assert.Contains("\"X\": 10", jsonContent);
        Assert.Contains("\"Z\": 20", jsonContent);
        Assert.Contains("\"Y\": 30", jsonContent);
        Assert.Contains("\"ElementName\": \"Square1\"", jsonContent);

        // Cleanup
        File.Delete(testFilePath);
    }*/
    
    }











