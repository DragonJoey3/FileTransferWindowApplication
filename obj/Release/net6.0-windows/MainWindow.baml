   M S B A M L   `   `   `  ���� X  TFileTransferWindowApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:FileTransferWindowApplicationFileTransferWindowApplication  .    (FileTransferWindowApplication.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     =local+clr-namespace:FileTransferWindowApplication     -   $��_root��  � Title$  File Transfer App��$	��485��$	��800�� � Icon$ 	Icon1.ico�� � 
ResizeMode$ NoResize=�.��� .+��� -   .@��� $��Import From Phone��.���� -    ��Name$ 	mainPanel�� �� Orientation$ Vertical=�.+��� $��10q�$��Center=�$��26(�-+Welcome to Joe's Photo Transfer Application�� $��10q�$��Center=�][This application is designed to make it easy to move files from your phone to your computer�� ECPlease ensure that your phone is plugged into the computer via USB.�� �� -   $��NoDevDisplay��.+��� $��Center=�$
��0,10q�
���  ��$��20(�No devices found.�� -   $��Refresh Device List��$	��150��
��������� -   $��DeviceDisplay��$y�	Collapsed=�$ 
Horizontal=�$��Center=�$��0,10,0,0q�.+��� $��Center=�$
��0,10q�$��20(�Phone found.�� -   $��TMDevDisplay��$y�	Collapsed=�$ Vertical=�$��Center=�.+��� $��Center=�$
��0,10q�
���  ��$��20(�75Too many devices found. Please plugin only the phone.�� -   $��Refresh Device List��$	��150��
��������� $
��0,10q��� $��Center=�$��18(�Trip Information�� $ 
Horizontal=�$��Center=�.+��� $��	Trip Name��$��Center=��� -	   $��TripName��$	��150��$��10q��� $ 
Horizontal=�$��Center=�.+��� $��Start Date:��$��Center=�(  "System.Windows.Controls.DatePicker  -
   $��	StartDate��$	��200��$��10q��� $ 
Horizontal=�$��Center=�.+��� $��	End Date:��$��Center=�  -   $��EndDate��$	��200��$��10q��� -   $��ipbutton��$��Import Photos��$	��150��
�������� -    �Name$ ProgressSection��$��20q�$y�	Collapsed=�.+�� -    �Name$ pbStatus��$y�Visible=�$��20q�$��20�� � IsIndeterminate$
 true�� � Maximum$ 1Q�	 � Minimum$	 0Q��� -   $��pbText��$��
Loading...��$��Center=�$��Center=��� $��Export to USB��.���� -   $ usbMainPanel��$ Vertical=�.+��� $��10q�$��Center=�$��26(�-+Welcome to Joe's Photo Transfer Application�� $��10q�$��Center=�VTThis tab is designed to make it easy to move files already imported to your computer�� ;9to a USB. Start by selecting a trip and then a USB drive.�� �� $ 
Horizontal=�$��Center=�.+��� $��Center=�$&�� Select a USB drive to write to: ���� -   $��DriveDD��$��30��$��10q�
 �� SelectedItem
 ��  �� ElementName$ _root�� �� Path$ SelectedDrive� �� Mode$ TwoWay=��� -   $��5q�$��Center=�$��Refresh���� $
��0,10q��� $ 
Horizontal=�$��Center=�.+��� $��Center=�$��Select a trip to export: ���� -   $��TripsDD��$��10q�$	��160��
 �� $ _root��$ SelectedTrip�$ TwoWay=��� $
��0,10q��� -   $��epbutton��$��Export Photos��$	��150��
�������� -   $ ExProgressSection��$��20q�$y�	Collapsed=�.+�� -   $ 
exPbStatus��$y�Visible=�$��20q�$��20��$
 true��$ 1Q�$	 0Q��� -   $��exPbText��$��
Loading...��$��Center=�$��Center=��� $��Bottom=�$��Center=�53Copyright © Joseph David Furches, 2023 Version 2.0