export interface Member{
    id?: number;
    email?: string;
    username?: string;
    memberProfile?: {
        birthday?: string;
        age?: number,
        gender?: string;
        description?: string;
        memberPhoto: {
            id: number;
            url: string;
        }
        countryIso?:{
            id?: number;
            name?: string;
            twoLetterCode?: string;
            threeLetterCode?: string;
            numericCode?: string;
        },
        memberData?: {
            upvotes?: number;
            downvotes?: number;
            followers?: number[];
            following?: number[];
            userFavoriteGames?: number[];
            finishedLobbies?: number[];
        }
        memberCustomization: {
            backgroundUrl?: string;
        }
    }
}