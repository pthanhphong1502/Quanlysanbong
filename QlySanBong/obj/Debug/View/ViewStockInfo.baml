   M S B A M L   `   `   `  ����E  AQlySanBong, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:QlySanBong.ViewQlySanBong.View  5   	   A%clr-namespace:QlySanBong.ResourceXAMLQlySanBong.ResourceXAML  65   #    QlySanBong.View.ViewStockInfo   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   :d2http://schemas.microsoft.com/expression/blend/2008  5   	   Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5   	   )localclr-namespace:QlySanBong.View  5   	   .uc%clr-namespace:QlySanBong.ResourceXAML  65   -   5          �Name$  wdViewStockInfo��5	   	    � WindowStyle$
 None=�5
      
������  � AllowsTransparency .  � WindowStartupLocation$ CenterScreen=�5   	    � Title$ ViewStockInfo��5   	   $	��300��6   $	��500��6,   .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      .+�5   
   � $��0��6   .+�5      /   )QlySanBong.ResourceXAML.controlbarControl  
 � Tag 6#   �� *Title+ �� ElementName$ wdViewStockInfo��5      5      �� 5   
   $��1��6   
������� �� CornerRadius$ 	0,0,10,10��61   ._�5      � $��1��6    5      �� 5      $
��Auto �6$   �� 5      .+�5      �� $��/Image/ghim.png��6   $��50��61   $��50��6=   	 K� ZIndex$	 2��6H   $��407,21,43,199q�6Y   $��2��6p   #  MaterialDesignThemes.Wpf.Card  5      $��1��6&   
�������
 J� RenderTransformOrigin$
 0.5,0.5&�6L   $	��200��6m   $	��400��6z      UniformCornerRadius$ 20Q�6�   .��5       ��  �� Orientation$ Vertical=�6!   $	��200��68   $	��300��6E    � 	Resources 5!       �� 
TargetType �� Property �� Value��%R   '�       �� 5"      " �6$   .�5#   "   ��  � ��$ 10q�6;   5$      5%      .+�5&      � .+�5'      �� $��Mã hóa đơn��6(   $��15(�6:   $
��Left=�6H   �� 5(      -   5         ��Name$ 
txbIdStock��5(   (   $
��Bold"�6<   $��Right=�6N   5)      � 5*      .+�5+      �� $��Mã Hàng hóa��6(   $��15(�6;   $
��Left=�6I   �� 5,      -   5        $ 	txbIdGood��5,   (   $
��Bold"�6;   $��Right=�6M   5-      � 5.      .+�5/      �� $ ��Số lượng sản phẩm��6(   $��15(�6A   $
��Left=�6O   �� 50      -   5        $ txbQuantity��50   (   $
��Bold"�6=   $��Right=�6O   51      � 52      .+�53      �� $��Giá sản phảm��6(   $��15(�6<   $
��Left=�6J   �� 54      -   5        $ txbImportPrice��54   (   $
��Bold"�6A   $��Right=�6S   55      57      58      59      5:      5;      5<      