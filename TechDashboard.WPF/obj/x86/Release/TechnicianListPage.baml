   M S B A M L   `   `   `  ���� S  OTechDashboard.WPF, Version=1.6.6095.25312, Culture=neutral, PublicKeyToken=null5clr-namespace:TechDashboard.WPFTechDashboard.WPF  *    $TechDashboard.WPF.TechnicianListPage   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     1localclr-namespace:TechDashboard.WPF       � 	Resources  *  $System.Windows.Controls.DataGridCell �� 
TargetType �� Property �� Value �� 
TargetType �� Padding J� SnapsToDevicePixels��%�   '        �� "  .���  � ��$ 30q���  � �� �� "  .R��� # z��#��z��#��z��#��z�� . ._��� # z .��� .+�&   System.Windows.Controls.DataGrid  -     Name$ gridTechnician��   AutoGenerateColumns .  	   AlternatingRowBackground
	 �����
   HeadersVisibility$

 None=�   GridLinesVisibility$
 None=�$��Stretch=�$��0,60,0,0q�   RowStyle �� )  #System.Windows.Controls.DataGridRow"  .���  � �� �� 	BaseColor�� �� Color$ #ECF0F1��   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ *    CellTemplate �� .R���  �� Orientation$ Vertical=�.+��� $
��Bold"�$��18(����#7f8c8d���� *FormattedTechnicianNumber+�� $ 
Horizontal=�.+��� $
��Bold"�$��16(����#7f8c8d���� *
LastName+�� $
��Bold"�$��16(����#7f8c8d�� $�, ���� $
��Bold"�$��16(����#7f8c8d���� *	FirstName+�� $��60��$	��Top=����#3498DB���	#FF3498DB�� -    ��Name$ 
labelTitle��$��SELECT TECHNICIAN��$��34��
�������$��
83,17,78,0q�$��Center=�$	��Top=�$��13.333(�$
��Bold"�