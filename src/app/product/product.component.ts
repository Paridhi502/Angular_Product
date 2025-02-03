import { Component, Inject, OnInit } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-products',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductsComponent implements OnInit {
  products: any[] = [];
  currentProduct = { productId: 0, name: '', description: '', price: 0, releaseDate: '' };
  isEditing = false;

  constructor(@Inject(ProductService) private productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts() {
    this.productService.getProducts().subscribe((data: any[]) => {
      this.products = data;
    });
  }

  addProduct() {
    this.productService.addProduct(this.currentProduct).subscribe(() => {
      this.loadProducts();  // Reload products to reflect changes
      this.resetForm();
      alert('Product added successfully!');
    });
  }

  editProduct(product: any) {
    // Debugging log to see the product data
    console.log("Editing product:", product);

    // Convert the releaseDate to a valid format for the <input type="date">
    const formattedReleaseDate = new Date(product.releaseDate).toISOString().split('T')[0]; // Format to YYYY-MM-DD

    this.currentProduct = { 
      productId: product.productId, 
      name: product.name, 
      description: product.description, 
      price: product.price, 
      releaseDate: formattedReleaseDate 
    };
    console.log("Current Product after formatting:", this.currentProduct);  // Debugging log to check if the releaseDate is correct

    this.isEditing = true;
  }

  saveProduct() {
    if (this.isEditing) {
      this.productService.updateProduct(this.currentProduct.productId, this.currentProduct).subscribe(() => {
        this.loadProducts();  // Reload products to reflect changes
        this.resetForm();
        alert('Product updated successfully!');
      });
    } else {
      this.addProduct();
    }
  }

  deleteProduct(id: number) {
    this.productService.deleteProduct(id).subscribe(() => {
      this.loadProducts();  // Reload products to reflect changes
      this.resetForm();
      alert('Product deleted successfully!');
    });
  }

  cancelEdit() {
    this.resetForm();
  }

  resetForm() {
    this.currentProduct = { productId: 0, name: '', description: '', price: 0, releaseDate: '' };
    this.isEditing = false;
  }
}
