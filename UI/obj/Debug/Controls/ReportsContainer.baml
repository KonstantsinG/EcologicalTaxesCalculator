   M S B A M L   `   `   `  ����=  9UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:UI.ControlsUI.Controls  5      "    UI.Controls.ReportsContainer   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      +localclr-namespace:UI.Controls     5      .��5
      � $��5q�6     � ColumnDefinitions  5   
   �� 5      $��45 �6   �� 5       � RowDefinitions 5   
   �� 5      $��45 �6   �� 5      .+�5   
   �� -   5         ��Name$ selectionPanel��5      $��2��5      $
��Left=�6(    �� RadiusX$ 20��5       �� RadiusY$ 20��6!   $	��200��6.   $��-7,3,0,3q�6:   5      �� 5   
   -   5         ��Name$ imageViewer��5      $��5q�6   ��5      �� 5       �� 	UriSource$ /Images/folder.png?�6   -   5        
bitmap��5   !   5       �� 5"   
   -   5        $��folderNameTBlock��5$   B   $��WrapWithOverflow=�5#      $��1��65   $��15(�5$      $��Normal"�6#   $��5q�67   $��Center=�5%      ��5"      �� *Text+ �� NotifyOnTargetUpdated .  �� Mode$ TwoWay=�5%   0   �� 5'   
   -   5        	 ��Name$	 selectionBorder��5)      ���	#FFF4F4F4$��1��5'   (   $��Right=�5(      $��1��6-   $y�	Collapsed=�5)   *   �� 5+   
   -   5        
 ��Name$
 subfoldersContainer��5+      $��1��62   $��1��6?   $y�	Collapsed=�5,      .+�5.      �� -   5        $��5q�5.      $��30��6   $��30��6*   $
��Left=�66   ��50      �� 51      $ /Images/subfolder.png?�6"   53      �� 55      -   5         ��Name$ monthTBlock��58   4   $"��Отчёты за месяц��55      $��WrapWithOverflow=�56      $��15(�57      $��Light"�6'   $��
40,-40,0,0q�6:   $��Center=�58      64   �� 5:      -	   5         ��Name$ monthReportsContainer��5:      $��25,0,0,0q�6:   $y�	Collapsed=�5<      ��5;      �� *MonthReports+��5=      �� 5>      .R�5?      ��  �� Orientation$ Vertical=�6%   5@      5B      �� 5D      -
   5        $��5q�5D      $��30��6   $��30��6*   $
��Left=�66   ��5F      �� 5G      $ /Images/subfolder.png?�6"   5I      �� 5K      -   5        $ quartalTBlock��5N   4   $&�� Отчёты за квартал��5K      $��WrapWithOverflow=�5L      $��15(�5M      $��Light"�6'   $��
40,-40,0,0q�6:   $��Center=�5N      64   �� 5P      -   5        $ quartalReportsContainer��5P      $��25,0,0,0q�6<   $y�	Collapsed=�5Q   6   ��6   �� *QuartalReports+��5R      �� 5S      .R�5T      �� $ Vertical=�6%   5U      5W      �� 5Y      -   5        $��5q�5Y      $��30��6   $��30��6*   $
��Left=�66   ��5[      �� 5\      $ /Images/subfolder.png?�6"   5^      �� 5`      -   5        $ 
yearTBlock��5c   4   $��Отчёты за год��5`      $��WrapWithOverflow=�5a      $��15(�5b      $��Light"�6'   $��
40,-40,0,0q�6:   $��Center=�5c      64   �� 5e      -   5        $ yearReportsContainer��5e      $��25,0,0,0q�69   $y�	Collapsed=�5f   3   ��6   �� *YearReports+��5g      �� 5h      .R�5i      �� $ Vertical=�6%   5j      5l      5n      5p      5r      