   M S B A M L   `   `   `  ����L  HTechDashboard.WPF, Version=1.9.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:TechDashboard.WPFTechDashboard.WPF  5      &     TechDashboard.WPF.TechnicianPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:TechDashboard.WPF     5      .��5      � -   5          �Name$  gridMain��5      $��0q�6    � ColumnDefinitions 5   
   �� 5      $��* �6   .+�5   
   �� $��60��6   $	��Top=�6    ���#3498DB���	#FF3498DB6G   �� 5   
   -   5         ��Name$ 
labelTitle��5      $��TECHNICIAN DETAILS��6$   $��34��6A   
�������$��
83,17,78,0q�6`   $��Center=�6t   $	��Top=�6�   $��18(�6�   $
��Bold"�6�   �� 5   
   -   5        $ lblTechDeptNumber��5      $��TechnicianNo��6+   $
��Left=�6B   $��	10,65,0,0q�6]   $	��Top=�6p   ���	#FF7F8C8D6�   �� 5   
   -   5        $ lblTechnicianName��5      $��TechnicianName��6+   $
��Left=�6D   $��	10,85,0,0q�6_   $	��Top=�6r   ���	#FF7F8C8D6�   �� 5   
   -   5         ��Name$ pkrTechnicianStatus��5       �� DisplayMemberPath$ StatusDescription��60   $
��Left=�6V   $��
16,110,0,0q�6q   $	��Top=�6�   $	��245��6�   �� 5   
   -   5         ��Name$ btnOk��5      $��OK��6    ���#2ECC71
�������$��Stretch=�6n   $��10,140,12,0q�6�   $	��Top=�6�   $��50��6�   5      5      