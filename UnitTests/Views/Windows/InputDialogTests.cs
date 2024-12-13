// using System;
// using System.Windows;
// using System.Windows.Controls;
// using BPR2_Desktop.ViewModels;
// using BPR2_Desktop.Views.Pages;
// using BPR2_Desktop.Views.Windows;
// using Moq;
// using Xunit;
// using Wpf.Ui.Controls;
// using Button = Wpf.Ui.Controls.Button;
// using TextBox = Wpf.Ui.Controls.TextBox;
//
// namespace UnitTests
// {
//     public class InputDialogTests
//     {
//         public InputDialogTests()
//         {
//             if (Application.Current == null)
//             {
//                 new Application();
//             }
//         }
//
//         [StaFact]
//         public void OKButton_Click_SetsResponseTextAndCloses_WhenInputIsValid()
//         {
//             // Arrange
//             var viewModel = new InputDialogViewModel
//             {
//                 Prompt = "Test Prompt",
//                 ResponseText = "Valid Input"
//             };
//
//             // Act
//             var isValid = viewModel.IsInputValid;
//
//             // Assert
//             Assert.True(isValid);
//             Assert.Equal("Valid Input", viewModel.ResponseText);
//         }
//
//         [StaFact]
//         public void OKButton_Click_ShowsError_WhenInputIsInvalid()
//         {
//             // Arrange
//             var inputDialog = new InputDialog("Test Prompt");
//             var textBox = new TextBox { Text = string.Empty };
//             inputDialog.InputTextBox = textBox;
//             var button = new Button();
//             inputDialog.OKButton = button;
//
//             // Act
//             inputDialog.OKButton_Click(button, new RoutedEventArgs());
//
//             // Assert
//             Assert.Null(inputDialog.ResponseText);
//             Assert.Null(inputDialog.DialogResult);
//         }
//
//         [StaFact]
//         public void InputTextBox_TextChanged_EnablesOKButton_WhenInputIsValid()
//         {
//             // Arrange
//             var inputDialog = new InputDialog("Test Prompt");
//             var textBox = new TextBox { Text = "Valid Input" };
//             var button = new Button { IsEnabled = false };
//             inputDialog.InputTextBox = textBox;
//             inputDialog.OKButton = button;
//
//             // Act
//             inputDialog.InputTextBox_TextChanged(textBox,
//                 new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.None));
//
//             // Assert
//             Assert.True(inputDialog.OKButton.IsEnabled);
//         }
//     }
// }
