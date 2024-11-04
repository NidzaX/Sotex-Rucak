import { OrderDishDto } from "./OrderDishDto";
import { SideDishDto } from "./SideDishDto";

export interface NewOrderDto{
    dishes: OrderDishDto[];
    sideDishes: SideDishDto[];
}