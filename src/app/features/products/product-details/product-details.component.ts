import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../../../core/product.service';
import { CartService } from '../../../core/cart.service';
import { Product } from '../../../core/models/product.model';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css'],
})
export class ProductDetailsComponent implements OnInit {
  product!: Product;
  quantity = 1;

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.productService.getProductById(id).subscribe((res) => {
      this.product = res;
    });
  }

  addToCart(): void {
    const payload = {
      productId: this.product.id,
      quantity: this.quantity,
    };

    this.cartService.addToCart(payload).subscribe(() => {
      alert('Added to cart successfully');
    });
  }
}
