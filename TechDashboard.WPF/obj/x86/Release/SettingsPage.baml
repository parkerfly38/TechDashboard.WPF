   M S B A M L   `   `   `  ���� S  OTechDashboard.WPF, Version=1.6.6095.25312, Culture=neutral, PublicKeyToken=null5clr-namespace:TechDashboard.WPFTechDashboard.WPF  $    TechDashboard.WPF.SettingsPage   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     1localclr-namespace:TechDashboard.WPF     -     �Name$  techdashboardwindow�� � Title$ Tech Dashboard��$��422.65��$	��525�� � Icon$ td_sq_h8J_icon.ico��.��� .+��� $��-1,0,0,0q����	#FF3498DB���	#FF3498DB$��46��$	��Top=��� -    ��Name$ labelHeading��$��APP SETTINGS��$��212,10,137,0q�$	��Top=�$
��Bold"�
��������� -    ��Name$ cbxIsUsingHttps��$��http / https��$
��Left=�$��	10,51,0,0q�$	��Top=����	#FF7F8C8D d� 	IsChecked �� *IsUsingHttps+�� -   $ label��$
��URL:��$
��Left=�$��	10,77,0,0q�$	��Top=�$
��Bold"����	#FF7F8C8D�� -    ��Name$ textURL��$��
99,81,10,0q� �� TextWrapping$
 Wrap=�$��24��$	��Top=�$y�Hidden=����� *
SDataUrl+�� -   $ 
textUserID��$��
99,120,0,0q�$
 Wrap=�$	��148��$��24��$	��Top=�$
��Left=�$y�Hidden=����� *SDataUserId+G� -    G�Name$ passwordBox��$
��Left=�$��
99,154,0,0q�$	��Top=�$	��148��$��24��$y�Hidden=��� -   $ labelDaysBefore��$��Days Before:��$
��Left=�$��27��$��
10,113,0,0q�$	��Top=�$��80��$
��Bold"����	#FF7F8C8D�� -	   $ textDaysBefore��$
��Left=�$��24��$��
99,113,0,0q�$
 Wrap=�$	��Top=�$��79������ *ScheduleDaysBefore+�� -
   $ labelDaysAfter��$��Days After:��$
��Left=�$��24��$��
10,148,0,0q�$	��Top=�$��80��$
��Bold"����	#FF7F8C8D�� -   $ textDaysAfter��$
��Left=�$��24��$��
99,150,0,0q�$
 Wrap=�$	��Top=�$��79������ *ScheduleDaysAfter+�� -   	 ��Name$	 btnSaveAppSettings��$��SAVE SETTINGS��$��10,341,10,0q�$	��Top=�$��34�����	#FF2ECC71���	#FF2ECC71
�������$
��Bold"��� -   $ labelHttpText��$��http://��$
��Left=�$��	58,81,0,0q�$	��Top=�$��10.667(����	#FF7F8C8D�� -   $ textRESTURL��$��24��$��
99,79,10,0q�$
 Wrap=�$	��Top=����� *RestServiceUrl+�� -   $ checkBox��$��24 Hour Time��$
��Left=�$��
99,296,0,0q�$	��Top=�$��25�����	#FF7F8C8D �� *Use24HourTime+�� -   $ labelDaysAfter_Copy��$��Tech No:��$
��Left=�$��24��$��
10,181,0,0q�$	��Top=�$��80��$
��Bold"����	#FF7F8C8D�� -   $ labelDaysAfter_Copy1��$��
Tech Dept:��$
��Left=�$��24��$��
10,214,0,0q�$	��Top=�$��80��$
��Bold"����	#FF7F8C8D�� -   $ textUserID_Copy��$��
99,185,0,0q�$
 Wrap=�$	��148��$��24��$	��Top=�$
��Left=����� *LoggedInTechnicianNo+�� -   $ textUserID_Copy1��$��
99,218,0,0q�$
 Wrap=�$	��148��$��24��$	��Top=�$
��Left=����� *LoggedInTechnicianDeptNo+�� -   $ 
lblVersion��$��Label��$��
0,303,10,0q�$��26��$	��Top=�$��Right=�$��97���� -   $ labelDeviceName��$��Device Name:��$
��Left=�$��24��$��
10,252,0,0q�$	��Top=�$��88��$
��Bold"����	#FF7F8C8D�� -   $ textDeviceName��$��
99,252,0,0q�$
 Wrap=�$	��148��$��24��$	��Top=�$
��Left=����� *
DeviceName+