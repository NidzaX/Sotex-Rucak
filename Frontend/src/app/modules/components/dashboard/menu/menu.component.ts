import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MenuItemsComponent } from "./menu-items/menu-items.component";
import { CommonModule } from '@angular/common';
import { MenuService } from '../../../../core/services/menu.service';
import { AddMenuDto } from '../../../../core/models/AddMenuDto';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [FormsModule, MenuItemsComponent, CommonModule, RouterModule],
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  selectedFile: File | null = null;
  uploadedMenu: AddMenuDto | null = null;
  isMenuAvailable: boolean = false;

  @Output() menuStatusChanged = new EventEmitter<boolean>();

  constructor(private menuService: MenuService, private router: Router, private authService: AuthService) {} 

  ngOnInit(): void {
    this.checkMenuStatus();
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  checkMenuStatus() {
    this.menuService.checkMenuStatus().subscribe({
      next: (response: any) => {
        console.log(response);
        this.isMenuAvailable = response.isActive || response.isActiveTomorrow; 
        this.menuStatusChanged.emit(this.isMenuAvailable);
        if (!this.isMenuAvailable) {
          console.log('Menu is not available');
        }
      },
      error: (error) => console.error('Error checking menu status:', error)
    });
  }

  uploadMenu() {
    if (!this.selectedFile) {
      console.error('No file selected for upload.');
      return;
    }

    const formData = new FormData();
    formData.append('file', this.selectedFile);

    this.menuService.uploadMenu(formData).subscribe({
      next: (response: AddMenuDto) => {
        this.uploadedMenu = response;
        console.log("Menu uploaded successfully:", response);
        this.isMenuAvailable = true;
        this.menuStatusChanged.emit(this.isMenuAvailable);
      },
      error: (error) => console.error("Error uploading menu:", error)
    });
  }
}
