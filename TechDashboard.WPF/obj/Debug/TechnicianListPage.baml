   M S B A M L   `   `   `  ����L  HTechDashboard.WPF, Version=1.9.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:TechDashboard.WPFTechDashboard.WPF  5      *    $TechDashboard.WPF.TechnicianListPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:TechDashboard.WPF     5        � 	Resources  5	      *  $System.Windows.Controls.DataGridCell �� 
TargetType �� Property �� Value �� 
TargetType �� Padding J� SnapsToDevicePixels��%  '        �� 5   
   "  6   .�5      ��  � ��$ 30q�6(   �� 5       � �� 5      �� 5      "  6&   .R�5      �� # z��6!   #��z��5      #��z��5      #��z��5       . ._�5      �� # z 6/   5      5      5      5      5      .��5      � .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ gridTechnician��5         AutoGenerateColumns .  	   AlternatingRowBackground
	 �����
   HeadersVisibility$

 None=�5         GridLinesVisibility$
 None=�5      $��Stretch=�5       $��0,60,0,0q�6%      RowStyle 5!      �� 5"      )  #System.Windows.Controls.DataGridRow"  6   .�5#      ��  � �� 5$      �� 5%      	BaseColor�� �� Color$ #ECF0F1��6A   5'      5(         Columns 5*      4  .System.Windows.Controls.DataGridTemplateColumn  5+      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6)      CellTemplate 5,      �� 5-      .R�5.      ��  �� Orientation$ Vertical=�6)   .+�5/   "   �� $
��Bold"�6W   $��18(�6i   ���#7f8c8d��6,   �� *FormattedTechnicianNumber+6w   �� 50   "   $ 
Horizontal=�6-   .+�51   &   �� $
��Bold"�6J   $��16(�6\   ���#7f8c8d��60   �� *
LastName+6j   �� 52   &   $
��Bold"�60   $��16(�6B   ���#7f8c8d�� 6f   $�, ��6j   6x   �� 53   &   $
��Bold"�6K   $��16(�6]   ���#7f8c8d��60   �� *	FirstName+6k   54   #   55      56      58      5:      �� 5;   
   $��60��6   $	��Top=�6    ���#3498DB���	#FF3498DB6G   �� 5<   
   -   5         ��Name$ 
labelTitle��5<      $��SELECT TECHNICIAN��6$   $��34��6@   
�������$��
83,17,24,0q�6_   $��Center=�6s   $	��Top=�6�   $��18(�6�   $
��Bold"�6�   $	��193��6�   5=      5>      