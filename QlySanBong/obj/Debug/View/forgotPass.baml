   M S B A M L   `   `   `  ����E  AQlySanBong, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:QlySanBong.ViewQlySanBong.View  5   	   ;"clr-namespace:QlySanBong.ViewModelQlySanBong.ViewModel  5	   	   A%clr-namespace:QlySanBong.ResourceXAMLQlySanBong.ResourceXAML  6>        QlySanBong.View.forgotPass   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:QlySanBong.View     5   	   _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   2	viewmodel"clr-namespace:QlySanBong.ViewModel  5	   	   .uc%clr-namespace:QlySanBong.ResourceXAML  6>   c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5   	   -   5          �Name$  wdForgotPass��5   	    � Title$ Quên mật khẩu��5
   	   $	��900��5   	    � WindowStyle$
 None=�5   	   
������  � AllowsTransparency . $	��700��5   	   &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5   	      TextRenderingMode$
 Auto=�5   	      loginVM � DataContext# [  5   	    � WindowStartupLocation$ CenterScreen=�5   	   .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      .+�5   
   � $��0��6   .+�5      /   )QlySanBong.ResourceXAML.controlbarControl  
	 � Tag	 6#   �� *Title+
 �� ElementName$
 wdForgotPass��5      5      �� 5   
   $��1��6   
������� �� CornerRadius$ 	0,0,10,10��61   ._�5       � $��1��6   .+�5!      �� $��/Image/ball.png��6   $��Bottom=�61   $	��600��5"      $	��600��6   $��Right=�6)   $��0,0,-90,-90q�6E   � 5#      $��Center=�6   $��0,15,0,0q�64   $	��Top=�5$      .+�5%      � $	��600��6   $	��800��6'   $��Center=�64   $��Center=�6O    5&      �� 5'      $	��100 �6,   �� 5(      $	��600 �6,   �� 5)      $��60 �6,   .+�5+      � .+�5,      �� $��Quên mật khẩu��6$   $��Center=�5-      $��Center=�5.      $
��Bold"�5/      
�������$��Munich)�51      $��70(�52      $��0,10,0,0q�53      54      � 55      $��0,0,0,0q�6   $��1��56      $	��Top=�57       58      �� 59   "   $	��150 �60   �� 5:   "   $	��150 �60   �� 5;   "   $	��150 �60   �� 5<   "   $	��150 �60   .+�5@      � $��0��6#   .+�5A   "   �� $��Stretch=�6-   $��Center=�5B   %   $��	0,20,0,30q�5C   %    �� RadiusY$ 15��5D   %    �� RadiusX$ 15��62   
�������$	��427��5F   %   $	��100��61   �� 5G   "   -   5         ��Name$ 	txAccount��5G   *    ! MaterialDesignOutlinedTextBox#��[ 5H      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Tên đăng nhập��5I      $��Stretch=�5J      $��Center=�5K      $��	0,20,0,30q�5L      
���   �
������ $��35(�5O      $	��427��6,   '  !MaterialDesignThemes.Wpf.PackIcon  5P   "      Kind$ 
AccountBox=�6:   $��60��5Q   !   $��60��6-   $��Stretch=�5R   !   $
��Left=�5S   !   $��	10,20,0,0q�5T   !   
���   � J� Opacity$
 0.75Q�5U   4   5V      � 5W      $��1��6#   .+�5X   "   �� 
�������$��1��5Y   $   $ 15��5Z   $   $ 15��61   $	��427��5[   $   $	��100��60   $��Stretch=�5\   $   $��Center=�5]   $   $��	0,20,0,30q�5^   $   G� 5`   "   -   5         G�Name$ pwIdAccount��5`   .    % !MaterialDesignOutlinedPasswordBox#��[ 5a      $  Nhập mã ID tài khoản��5b      $��Stretch=�5c      $��Center=�5d      $��	0,20,0,30q�5e      
���   �$��0q�5g      $��35(�5h      $	��427��6,   .  (System.Windows.Interactivity.Interaction   Triggers 5i   &   /  )System.Windows.Interactivity.EventTrigger  5j   *      	EventName$ KeyUp��69   .  (System.Windows.Interactivity.TriggerBase   Actions. 5k   .   6  0System.Windows.Interactivity.InvokeCommandAction     Command 6D   �� *ShowIdAdmin+   CommandParameter 5l   @   �� $
 wdForgotPass��5m   +   5o   #     5p   "   $ Password=�6:   $��60��5q   !   $��60��6-   $��	10,20,0,0q�5r   !   $��Stretch=�5s   !   $
��Left=�5t   !   
���   �$
 0.75Q�5u   4   5w      � 5x      $��2��6#   .+�5y   "   �� 
�������$ 15��5z   $   $ 15��61   $��Center=�5{      $��Center=�5|      $	��427��5}      $	��100��6*   G� 5~   "   -   5        $ 	pwNewPass��5~   .   #��[ 5      $ Nhập mật khẩu mới��5�      $��Center=�5�      $��Center=�5�      $��	0,20,0,30q�5�      
���   �
������ $��35(�5�      $	��427��6,    5�   &     5�   *   $ PasswordChanged��69   . 5�   .      6D   �� *PasswordChangedCommand+ 5�   4   �� $
 	pwNewPass��5�   +   5�   #     5�   "   $ PasswordCheck=�6:   $��60��5�   !   $��60��6-   $��	10,20,0,0q�5�   !   $��Stretch=�5�   !   $
��Left=�5�   !   
���   �$
 0.75Q�5�   4   5�      � 5�      -   5         �Name$ 	grAccount��5�   0   $��3��6#   $y�Hidden=�6C   .+�5�   "   �� 
�������$ 15��5�   $   $ 15��61   $��Center=�5�      $��Center=�5�      $	��427��5�      $	��100��6*   G� 5�   "   -   5        $ pwCheckIdAdMin��5�      #��[ 5�      $! Nhập mã riêng cho Admin��5�      $��Center=�5�      $��Center=�5�      $��	0,20,0,30q�5�      
���   �
������ $��30(�5�      $	��427��6,     5�   "   $ PasswordQuestion=�6:   $��60��5�   !   $��60��6-   $��	10,20,0,0q�5�   !   $��Stretch=�5�   !   $
��Left=�5�   !   
���   �$
 0.75Q�5�   4   5�      5�      � 5�      $��1��6   $��25,571,25,0q�5�      $��83��5�      $��Center=�5�      $��Center=�5�      $��2��62   .+�5�      ��  �� Orientation$ 
Horizontal=�6)   .+�5�   "   �� $	��200��6)   $��50��5�      +	  %MaterialDesignThemes.Wpf.ButtonAssist 	  CornerRadius$ 20��5�      $��Xác nhận��5�      $��0q�5�       � Cursor$
 Hand��5�      
�������$��20(�5�   5   ��5�      �� *SaveNewCommand+��5�      �� $
 wdForgotPass��5�   #   �� 5�   "   $	��200��6)   $��50��5�      $��Trở lại��5�      $ 20��5�      $
 Hand��5�      
�������$��Right=�5�      $��Center=�5�   !   $��	150,0,0,0q�5�   !   $��20(�64   ���	#FFFF6961���	#FFFF6961��5�      �� *closeRegisterWindow+��5�      �� $
 wdForgotPass��5�   #   5�      5�      5�      �� 5�      
������� K� ZIndex$ -1��6-   
�������$ 20��6R   $ 20��6_   5�      5�      5�      5�      5�      