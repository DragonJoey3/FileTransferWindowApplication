   M S B A M L   `   `   `  ����X  TFileTransferWindowApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:FileTransferWindowApplicationFileTransferWindowApplication  5   	   .    (FileTransferWindowApplication.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   =local+clr-namespace:FileTransferWindowApplication     5   	   -   5        $��_root��5
   	     � Title$  File Transfer App��5   	   $	��485��6#   $	��800��60    � Icon$ 	Icon1.ico��6<    � 
ResizeMode$ NoResize=�5	   	   .��5      � .+�5   
   �� -   5        .@�5      �� $��Import From Phone��6   .��5      �� -   5         ��Name$ 	mainPanel��5       �� Orientation$ Vertical=�60   .+�5      �� $��10q�6    $��Center=�6,   $��26(�6I   -+Welcome to Joe's Photo Transfer Application6�   �� 5      $��10q�6    $��Center=�6,   ][This application is designed to make it easy to move files from your phone to your computer�� 6�   ECPlease ensure that your phone is plugged into the computer via USB.5      �� 5      �� 5      -   5        $��NoDevDisplay��5   !   .+�5      �� $��Center=�6%   $
��0,10q�6B   
���  ��$��20(�6a   No devices found.6�   �� 5      -   5        $��Refresh Device List��5   !   $	��150��6?   
�������6e   5      �� 5      -   5        $��DeviceDisplay��5   !   $y�	Collapsed=�66   $ 
Horizontal=�6M   $��Center=�6f   $��0,10,0,0q�6�   .+�5      �� $��Center=�6%   $
��0,10q�6B   $��20(�6P   Phone found.6l   5      �� 5      -   5        $��TMDevDisplay��5   !   $y�	Collapsed=�65   $ Vertical=�6L   $��Center=�6c   .+�5      �� $��Center=�6%   $
��0,10q�6B   
���  ��$��20(�6a   75Too many devices found. Please plugin only the phone.6�   �� 5      -   5        $��Refresh Device List��5   !   $	��150��6?   
�������6e   5      �� 5       $
��0,10q�6    �� 5!      $��Center=�6    $��18(�6=   Trip Information6]   �� 5"      $ 
Horizontal=�6!   $��Center=�6:   .+�5#      �� $��	Trip Name��6    $��Center=�64   �� 5$      -	   5        $��TripName��5$   "   $	��150��62   $��10q�6>   6L   5%      �� 5&      $ 
Horizontal=�6!   $��Center=�6:   .+�5'      �� $��Start Date:��6    $��Center=�66   (  "System.Windows.Controls.DatePicker  5(      -
   5        $��	StartDate��5(   %   $	��200��66   $��10q�6B   5)      �� 5*      $ 
Horizontal=�6!   $��Center=�6:   .+�5+      �� $��	End Date:��6    $��Center=�64     5,      -   5        $��EndDate��5,   %   $	��200��64   $��10q�6@   5-      �� 5.      -   5        $��ipbutton��5.   [   $��Import Photos��6   $	��150��65   
�������6k   � 5/      -   5         �Name$ ProgressSection��5/   '   $��20q�6   $y�	Collapsed=�6@   .+�50      � -   5         �Name$ pbStatus��50   ;   $y�Visible=�6&   $��20q�6M   $��20��6Y    � IsIndeterminate$
 true��51       � Maximum$ 1Q�52      	 � Minimum$	 0Q�53      �� 54      -   5        $��pbText��54   $   $��
Loading...��62   $��Center=�6D   $��Center=�6a   55      56      57      �� 58      $��Export to USB��6   .��59      �� -   5        $ usbMainPanel��59      $ Vertical=�63   .+�5:      �� $��10q�6    $��Center=�6,   $��26(�6I   -+Welcome to Joe's Photo Transfer Application6�   �� 5;      $��10q�6    $��Center=�6,   VTThis tab is designed to make it easy to move files already imported to your computer�� 6�   ;9to a USB. Start by selecting a trip and then a USB drive.5=      �� 5>      �� 5?      $ 
Horizontal=�6!   $��Center=�6:   .+�5@      �� $��Center=�6    $&�� Select a USB drive to write to: ��6;   6h   �� 5A      -   5        $��DriveDD��5A   =   $��30��6#   $��10q�61   
 �� SelectedItem
 6L   ��  �� ElementName$ _root�� �� Path$ SelectedDrive� �� Mode$ TwoWay=�5B      �� 5C      -   5        $��5q�5C   !   $��Center=�6,   $��Refresh��6G   6Y   5D      �� 5E      $
��0,10q�6    �� 5F      $ 
Horizontal=�6!   $��Center=�6:   .+�5G      �� $��Center=�6    $��Select a trip to export: ��6;   6a   �� 5H      -   5        $��TripsDD��5H   >   $��10q�6#   $	��160��6/   
 5I   #   �� $ _root��$ SelectedTrip�$ TwoWay=�5K      5L      �� 5M      $
��0,10q�6    �� 5N      -   5        $��epbutton��5N   [   $��Export Photos��6   $	��150��65   
�������6k   � 5O      -   5        $ ExProgressSection��5O   '   $��20q�6   $y�	Collapsed=�6B   .+�5P      � -   5        $ 
exPbStatus��5P   ;   $y�Visible=�6&   $��20q�6O   $��20��6[   $
 true��5Q      $ 1Q�5R      $	 0Q�5S      �� 5T      -   5        $��exPbText��5T   $   $��
Loading...��64   $��Center=�6F   $��Center=�6c   5U      5V      5W      5X      �� 5[   
   $��Bottom=�6   $��Center=�6/   53Copyright © Joseph David Furches, 2023 Version 2.06�   5\      5]      