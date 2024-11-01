import { DishDto } from "./DishDto";
import { OrderInfoDto } from "./OrderInfoDto";

export interface AddMenuDto {
    Day: string;
    Dishes: DishDto[];
    Sides: string[];
    SpecialOffer: string;
    OrderInfo: OrderInfoDto;
}