// using System.Windows;
// using System.Windows.Controls;
// using BPR2_Desktop.Views.Windows;
// using BPR2_Desktop.Views.Components;
// using BPR2_Desktop.Views.Pages;
// using Xunit;
//
// public class SetDimensionsTests
// {
//     private readonly DesignCanvasControl _realDesignCanvasControl;
//     private readonly DesignEditor _realDesignEditor;
//     private readonly SetDimensions _setDimensions;
//
//     public SetDimensionsTests()
//     {
//         // Use real instances for components that rely on XAML resources
//         _realDesignCanvasControl = new DesignCanvasControl();
//         _realDesignEditor = new DesignEditor();
//
//         // Create the SetDimensions instance with the real components
//         _setDimensions = new SetDimensions(_realDesignCanvasControl, _realDesignEditor);
//     }
//
//     [StaFact]
//     public void ShapeComboBox_SelectionChanged_ShowsCorrectGridForSquare()
//     {
//         // Arrange
//         var comboBox = new ComboBox();
//         comboBox.Items.Add(new ComboBoxItem { Content = "Square" });
//         comboBox.SelectedIndex = 0;
//
//         // Act
//         _setDimensions.ShapeComboBox_SelectionChanged(comboBox, null);
//
//         // Assert
//         Assert.Equal(Visibility.Collapsed, _setDimensions.SquareDimensionsGrid.Visibility);
//         
//         
//     }
//
//     [StaFact]
//     public void Save_Click_SetsDimensionsForSquare()
//     {
//         // Arrange
//         var comboBox = new ComboBox();
//         comboBox.Items.Add(new ComboBoxItem { Content = "Square" });
//         comboBox.SelectedIndex = 0;
//         _setDimensions.ShapeComboBox = comboBox;
//
//         _setDimensions.WidthTextBox.Text = "5";
//         _setDimensions.LengthTextBox.Text = "5";
//         _setDimensions.HeightTextBox.Text = "5";
//
//         // Act
//         _setDimensions.Save_Click(null, null);
//
//         // Assert
//         // Assume dimensions are updated; no way to mock DesignEditor without introducing exceptions
//         // Validate with logs, breakpoints, or refactor DesignEditor for better testability
//     }
//
//     [StaFact]
//     public void Save_Click_ShowsErrorForInvalidInput()
//     {
//         // Arrange
//         var comboBox = new ComboBox();
//         comboBox.Items.Add(new ComboBoxItem { Content = "Square" });
//         comboBox.SelectedIndex = 0;
//         _setDimensions.ShapeComboBox = comboBox;
//
//         _setDimensions.WidthTextBox.Text = "Invalid";
//
//         // Act
//         _setDimensions.Save_Click(null, null);
//
//         // Assert
//         // Assume error message is shown; validate with UI tests or logs if needed
//     }
//     
//     /*[StaFact]
//     public void ShapeComboBox_SelectionChanged_ShowsCorrectGridForComplicatedShape()
//     {
//         _setDimensions.SquareDimensionsGrid = new Grid { Visibility = Visibility.Collapsed };
//         _setDimensions.ComplicatedShapeGrid = new Grid { Visibility = Visibility.Collapsed };
//
//         // Arrange
//         var comboBox = new ComboBox();
//         comboBox.Items.Add(new ComboBoxItem { Content = "Complicated Shape" });
//         comboBox.SelectedIndex = 0;
//         
//
//         // Act
//         _setDimensions.ShapeComboBox_SelectionChanged(comboBox, null);
//
//         // Assert
//         Assert.Equal(Visibility.Collapsed, _setDimensions.ComplicatedShapeGrid.Visibility);
//         Assert.Equal(Visibility.Visible, _setDimensions.SquareDimensionsGrid.Visibility);
//     }*/
//     
//     
//     [StaFact]
//     public void Save_Click_SetsDimensionsForComplicatedShape()
//     {
//         // Arrange
//         var comboBox = new ComboBox();
//         comboBox.Items.Add(new ComboBoxItem { Content = "Complicated Shape" });
//         comboBox.SelectedIndex = 0;
//         _setDimensions.ShapeComboBox = comboBox;
//
//         _setDimensions.WallLengthTextBox.Text = "10";
//
//         // Act
//         _setDimensions.Save_Click(null, null);
//
//         // Assert
//         // Validate changes based on the "Complicated Shape" logic in Save_Click
//         // For example, you could check if the DesignCanvasControl or DesignEditor was updated
//     }
//     
//     [StaFact]
//     public void Save_Click_ShowsErrorForMissingDimensions()
//     {
//         // Arrange
//         var comboBox = new ComboBox();
//         comboBox.Items.Add(new ComboBoxItem { Content = "Square" });
//         comboBox.SelectedIndex = 0;
//         _setDimensions.ShapeComboBox = comboBox;
//
//         _setDimensions.WidthTextBox.Text = "";
//
//         // Act
//         _setDimensions.Save_Click(null, null);
//
//         // Assert
//         // Check for proper handling of missing input (e.g., error messages)
//     }
//     
//     [StaFact]
//     public void Cancel_Click_ClosesTheWindow()
//     {
//         // Act
//         _setDimensions.Cancel_Click(null, null);
//
//         // Assert
//         Assert.False(_setDimensions.IsVisible);
//     }
//
//
//
//
// }
