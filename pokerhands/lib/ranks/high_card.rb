require 'ranks/base_rank'

class HighCard < BaseRank
	
	attr_accessor :highest_card

	def name
		"high card"
	end

	def self.rank
		1
	end

	def <=> other
		if(other.is_a? HighCard)
			smart_compare other
		else
			super
		end
	end

	def smart_compare other
		my_sorted_cards = cards.sort
		other_sorted_cards = other.cards.sort

		my_sorted_cards.zip(other_sorted_cards).reverse_each { |e, f| 
			if(e > f)
				self.highest_card = e
				return 1
			elsif (e < f)
				other.highest_card = f
				return -1
			end	
		}
	end

	def to_s
		"#{name}: #{highest_card}"
	end
end