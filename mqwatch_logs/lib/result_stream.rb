class ResultStream



	def << line
		@last_line = line
	end

	def last_line
		@last_line || ""
	end
end