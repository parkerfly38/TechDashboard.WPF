   M S B A M L   `   `   `  ����L  HTechDashboard.WPF, Version=1.9.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:TechDashboard.WPFTechDashboard.WPF  5   	   $    TechDashboard.WPF.SettingsPage   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:TechDashboard.WPF     5   	   -   5          �Name$  techdashboardwindow��5   	    � Title$ Tech Dashboard��5   	   $	��430��6    $	��525��6-    � Icon$ td_sq_h8J_icon.ico��69   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $
��395* �6   �� 5      $
��126* �6   .+�5   
   �� $��-1,0,0,0q�6   ���	#FF3498DB���	#FF3498DB$��46��6J   $	��Top=�6V   $��2��6n   �� 5   
   -   5         ��Name$ labelHeading��5      ��.  $��APP SETTINGS��68   $��212,10,11.5,0q�6O   $	��Top=�6f   $
��Bold"�6~   
�������$��18(�6�   �� 5   
   -   5         ��Name$ cbxIsUsingHttps��5      $��http / https��6,   $
��Left=�6C   $��	10,51,0,0q�6^   $	��Top=�6q   ���	#FF7F8C8D d� 	IsChecked 6�   �� *IsUsingHttps+6�   �� 5   
   -   5        $ label��5      $	��URL��6   $
��Left=�6-   $��	10,77,0,0q�6H   $	��Top=�6[   $
��Bold"�6s   ���	#FF7F8C8D6�   �� 5   
   -   5         ��Name$ textURL��5      $��
99,81,10,0q�6#    �� TextWrapping$
 Wrap=�67   $��24��6K   $	��Top=�6W   $y�Hidden=�6�   $��2��6�   ��6o   �� *
SDataUrl+6�   �� 5   
   -   5        $ 
textUserID��5      $��
99,120,0,0q�6&   $
 Wrap=�6:   $	��148��6N   $��24��6Z   $	��Top=�6f   $
��Left=�6~   $y�Hidden=�6�   ��6�   �� *SDataUserId+6�   G� 5   
   -   5        	 G�Name$	 passwordBox��5      $
��Left=�6+   $��
99,154,0,0q�6F   $	��Top=�6Z   $	��148��6r   $��24��6~   $y�Hidden=�6�   �� 5   
   -   5        $ labelDaysBefore��5      $��Days Before��6)   $
��Left=�6?   $��27��6Z   $��
10,113,0,0q�6f   $	��Top=�6z   $��80��6�   $
��Bold"�6�   ���	#FF7F8C8D6�   �� 5   
   -	   5        $ textDaysBefore��5      $��1��6*   $
��Left=�67   $��24��6R   $��
99,113,0,0q�6^   $
 Wrap=�6r   $	��Top=�6�   $��79��6�   ��6�   �� *ScheduleDaysBefore+�� 5   
   -
   5        $ labelDaysAfter��5      $��
Days After��6(   $
��Left=�6=   $��24��6X   $��
10,148,0,0q�6d   $	��Top=�6x   $��80��6�   $
��Bold"�6�   ���	#FF7F8C8D6�   �� 5   
   -   5        $ textDaysAfter��5      $��2��6)   $
��Left=�66   $��24��6Q   $��
99,150,0,0q�6]   $
 Wrap=�6q   $	��Top=�6�   $��79��6�   ��6�   �� *ScheduleDaysAfter+�� 5   
   -   5        $ labelHttpText��5      $��http://��6'   $
��Left=�69   $��	58,81,0,0q�6T   $	��Top=�6g   $��10.667(�6   ���	#FF7F8C8D6�   �� 5   
   -   5        $ textRESTURL��5      $��24��6'   $��
99,79,10,0q�63   $
 Wrap=�6G   $	��Top=�6[   $��0��6�   $��2��6�   ��6s   �� *RestServiceUrl+6�   �� 5   
   -   5        $ checkBox��5      $��6��6%   $��24 Hour Time��62   $
��Left=�6I   $��
99,296,0,0q�6d   $	��Top=�6x   $��25��6�   ���	#FF7F8C8D 6�   �� *Use24HourTime+�� 5   
   -   5        $ labelDaysAfter_Copy��5      $��Tech No��6-   $
��Left=�6?   $��24��6Z   $��
10,181,0,0q�6f   $	��Top=�6z   $��80��6�   $
��Bold"�6�   ���	#FF7F8C8D6�   �� 5   
   -   5        $ labelDaysAfter_Copy1��5      $��	Tech Dept��6.   $
��Left=�6B   $��24��6]   $��
10,214,0,0q�6i   $	��Top=�6}   $��80��6�   $
��Bold"�6�   ���	#FF7F8C8D6�   �� 5   
   -   5        $ textUserID_Copy��5      $��3��6+   $��
99,185,0,0q�68   $
 Wrap=�6L   $	��148��6`   $��24��6l   $	��Top=�6x   $
��Left=�6�   ��6�   �� *LoggedInTechnicianNo+�� 5    
   -   5        $ textUserID_Copy1��5       $��4��6,   $��
99,218,0,0q�69   $
 Wrap=�6M   $	��148��6a   $��24��6m   $	��Top=�6y   $
��Left=�6�   ��6�   �� *LoggedInTechnicianDeptNo+�� 5!   
   -   5        $ 
lblVersion��5!      $��Label��6$   $��
0,293,10,0q�64   $��26��6H   $	��Top=�6T   $��Right=�6l   $��97��6�   $��1��6�   �� 5"   
   -   5        $ labelDeviceName��5"      $��Device Name��6)   $
��Left=�6?   $��24��6Z   $��
10,252,0,0q�6f   $	��Top=�6z   $��88��6�   $
��Bold"�6�   ���	#FF7F8C8D6�   �� 5#   
   -   5        $ textDeviceName��5#      $��5��6*   $��
99,252,0,0q�67   $
 Wrap=�6K   $	��148��6_   $��24��6k   $	��Top=�6w   $
��Left=�6�   ��6�   �� *
DeviceName+�� 5$   
   -   5        
 ��Name$
 btnSaveAppSettings��5$      ��.  $��SAVE SETTINGS��6?   $��10,319,10,0q�6W   $	��Top=�6l   $��34��6�   ���	#FF2ECC71���	#FF2ECC71
�������$
��Bold"�6�   $��2��6�   �� 5%   
   -   5        $
 btnCancelAppSettings��5%      ��.  $��CANCEL��6A   $��10,358,10,0q�6R   $	��Top=�6g   $��34��6   
���  �����	#FF2ECC71
�������$
��Bold"�6�   $��2��6�   5'      5(      