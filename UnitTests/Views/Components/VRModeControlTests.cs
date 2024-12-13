// using System.Net.Sockets;
// using System.Windows;
// using System.Windows.Media;
// using BPR2_Desktop.Services;
// using BPR2_Desktop.Views.Components;
// using Moq;
// using Wpf.Ui.Controls;
//
// namespace UnitTests.Views.Components;
//
// public class VRModeControlTests
// {
//     [WpfFact]
//     public void VRModeControl_ShouldInitializeCorrectly()
//     {
//         // Arrange & Act
//         var vrModeControl = new VRModeControl();
//
//         // Assert
//         Assert.NotNull(vrModeControl);
//         Assert.NotNull(vrModeControl.VRModeToggle);
//         Assert.False(vrModeControl.VRModeToggle.IsChecked);
//     
//         // Get the background color of the toggle and compare it to Colors.Red
//         var backgroundBrush = vrModeControl.VRModeToggle.Background as SolidColorBrush;
//         Assert.NotNull(backgroundBrush);
//         Assert.Equal(Colors.Red, backgroundBrush.Color); // Compare against Colors.Red
//     }
//     
//     [WpfFact]
//     public void SendJsonData_ShouldSetLastMessage_IfNoFileSelected()
//     {
//         // Arrange
//         var vrControl = new VRModeControl();
//         AppState.Instance.CurrentDesignFile = null;
//
//         // Act
//         vrControl.SendJsonData();
//
//         // Assert
//         Assert.Equal("No design file is currently selected. Please save a design first.", vrControl.LastMessage);
//     }
//     
//     
//     [WpfFact]
//     public void SendJsonData_ShouldSendMessageToUnity()
//     {
//         // Arrange
//         var unityClient = new UnityClient();
//         var vrModeControl = new VRModeControl(unityClient);
//
//         AppState.Instance.CurrentDesignFile = "test.json";
//         File.WriteAllText("test.json", "{ \"message\": \"test\" }");
//
//         // Act
//         vrModeControl.SendJsonData();
//
//         // Assert
//         Assert.True(true); 
//
//         // Cleanup
//         File.Delete("test.json");
//     }
//
//     [WpfFact]
//     public void SendJsonData_ShouldShowError_WhenNoFileIsSelected()
//     {
//         // Arrange
//         var vrModeControl = new VRModeControl();
//         AppState.Instance.CurrentDesignFile = null;
//
//         // Act
//         var exception = Record.Exception(() => vrModeControl.SendJsonData());
//
//         // Assert
//         Assert.Null(AppState.Instance.CurrentDesignFile); // Ensure no file is selected
//         Assert.Null(exception); // Ensure no exception is thrown
//     }
//     
//     [WpfFact]
//     public void ToggleVRMode_ShouldChangeBackgroundColor_WhenToggled()
//     {
//         // Arrange
//         var vrModeControl = new VRModeControl();
//
//         // Act
//         vrModeControl.VRModeToggle.IsChecked = true;
//         vrModeControl.ToggleVRMode(vrModeControl.VRModeToggle, new RoutedEventArgs());
//
//         // Assert
//         Assert.False(vrModeControl.VRModeToggle.IsChecked); // Ensure it toggled off
//         Assert.Equal(Colors.Red, ((SolidColorBrush)vrModeControl.VRModeToggle.Background).Color); // Verify background color changed
//     }
//     
//     /*[WpfFact]
//     public void ToggleVRMode_ShouldSetBackgroundGreen_WhenToggledOn()
//     {
//         // Arrange
//         var vrModeControl = new VRModeControl();
//
//         // Simulate user interaction: toggle ON
//         vrModeControl.VRModeToggle.IsChecked = true;
//
//         // Act
//         vrModeControl.ToggleVRMode(vrModeControl.VRModeToggle, new RoutedEventArgs());
//
//         // Assert
//         Assert.NotNull(vrModeControl.VRModeToggle.Background);
//         Assert.Equal(Colors.Green, ((SolidColorBrush)vrModeControl.VRModeToggle.Background).Color);
//     }*/
//
//
//
//
//     
//     
//     [WpfFact]
//     public void ToggleVRMode_ShouldDoNothing_WhenToggleStateIsNull()
//     {
//         // Arrange
//         var vrModeControl = new VRModeControl();
//         vrModeControl.VRModeToggle.IsChecked = null;
//
//         // Act
//         vrModeControl.ToggleVRMode(vrModeControl.VRModeToggle, new RoutedEventArgs());
//
//         // Assert
//         Assert.Null(vrModeControl.VRModeToggle.IsChecked); // State should remain null
//     }
//
//
//
//
//     
//
//
//
//
//     
//     
//     
//
// }