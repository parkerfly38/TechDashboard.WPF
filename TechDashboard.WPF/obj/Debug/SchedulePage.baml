   M S B A M L   `   `   `  ����L  HTechDashboard.WPF, Version=1.9.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:TechDashboard.WPFTechDashboard.WPF  5      $    TechDashboard.WPF.SchedulePage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:TechDashboard.WPF     5        � 	Resources  5	         placeHolder �� 
TargetType �� BasedOn �� Property �� Value �� 
TargetType ��Name K� ZIndex �� Path	 �� RelativeSource
 �� Mode �� UpdateSourceTrigger
 � Tag �� Triggers �� Binding �� Source%  System.Windows.Markup.Reference �� Value*  $System.Windows.Controls.DataGridCell �� Padding J� SnapsToDevicePixels��%�  &	        7��'  l    �� 5
   
   " ��6$   8   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      � .+�5      �� $ 
textSource��5   "   
������ $ 2��5   "   ��5   &   �� $
 Text�	 �� *TemplatedParent+$
 TwoWay=�$ PropertyChanged=�5   "   �� 5      #��z 6&   #��z��6C   $ 1��6m   .��6~   ��5   "   �� 5   &   " ��6,   .�5   *   ��  � ��
 ���� 6G    5   *   �� 5   .    6:   �� $
 Text�   *
textSource+  6y   .a�5   2   ��  � ��
 �����6O   5   /   5   '   .��5    1   5!      5"      5#      5%      5&      �� 5(   
   "  6   .�5)      ��  � ��$ 30q�6(   �� 5*       � �� 5+      �� 5,      "  6&   .R�5-      �� # z��6!   #��z��5.      #��z��5/      #��z��50       . ._�52      �� # z 6/   53      54      56      57      58      .��59      � .+�5:   
   &   System.Windows.Controls.DataGrid  -   5          Name$ gridSchedule��5:         AutoGenerateColumns .     AlternatingRowBackground
 �����   HeadersVisibility$
 None=�5;         GridLinesVisibility$
 None=�5<      -  'System.Windows.Input.KeyboardNavigation   TabNavigation$ Cycle=�5=      $��Stretch=�5>      $��Stretch=�5?      $��	0,153,0,0q�61      RowStyle 5@      �� 5A      )  #System.Windows.Controls.DataGridRow"  6   .�5B      ��  � �� 5C      �� 5D      	BaseColor�� �� Color$ #ECF0F1��6A   6S   5F      5G         Columns 5I      4  .System.Windows.Controls.DataGridTemplateColumn  5J      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6)      CellTemplate 5K      �� 5L      .R�5M      ��  �� Orientation$ 
Horizontal=�6)   .+�5N   "   �� $ Vertical=�6-   .+�5O   &   �� $
��Bold"�6U   $��18(�6g   ���#7f8c8d��60   �� *ServiceTicketNumber+6�   �� 5P   &   $ Vertical=�61   .+�5Q   *   �� $
��Bold"�6X   $��16(�6j   ���#7f8c8d��64   �� *SchedDateStartTime+6�   �� 5R   *   $
��Bold"�6R   $��16(�6d   ���#7f8c8d��64   �� *NameLocPhone+6�   5S   '   5T   #   �� 5U   "   ��6(   �� *CurrentImageFileName+5V      5W      5Y      5[      �� 5]   
   $	��160��6   $	��Top=�6!   ���#3498DB���	#FF3498DB$��Stretch=�6[   �� 5^   
   -   5         ��Name$ 
labelTitle��5^      $��SELECT SERVICE TICKET��6$   ��.  $��34��6V   
�������$��	83,1,78,0q�6u   $��Center=�6�   $	��Top=�6�   $��18(�6�   $
��Bold"�6�   (	  "System.Windows.Controls.DatePicker	  5_   
   -   5          	 Name$  filterStartDate��5_      $��
10,35,10,0q�6.   $	��Top=�6B   ! 	  DisplayDate! 6Z   �� *DefaultStartDate+" 	  SelectedDate" 6�   �� *DefaultStartDate+	  5`   
   -   5        $  filterEndDate��5`      $��
10,64,10,0q�6,   $	��Top=�6@   ! 6X   �� *DefaultEndDate+" 6   �� *DefaultEndDate+�� 5a   
   -   5        # ��Name$# buttonFilter��5a      $��
10,88,10,0q�6'   ��.  $	��Top=�6M   ���	#FF2ECC71
����������	#FF2ECC71$$��FILTER SERVICE TICKETS BY DATE��6�   $
��Bold"�6�   $��33��6�   $��16(�6	  �� 5b   
   -   5        $ txtSTFilter��5b      #��[  6'   $ Filter on Ticket Number��6L   $��24��6j   $��10,128,10,0q�6v   $	��Top=�6�   5c      5d      